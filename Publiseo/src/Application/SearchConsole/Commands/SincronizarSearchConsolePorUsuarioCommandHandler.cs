using Application.SearchConsole.Abstractions;
using Application.SearchConsole.Contracts;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Commands;

[ExcludeFromCodeCoverage]
public sealed class SincronizarSearchConsolePorUsuarioCommandHandler : IRequestHandler<SincronizarSearchConsolePorUsuarioCommand, SincronizarSearchConsoleResult>
{
    private const string TipoBuscaPadrao = "web";
    private const int DiasAtrasPadrao = 3;
    private const int PrimeiraSincronizacaoDias = 90;
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly ISearchConsoleClient _searchConsoleClient;
    private readonly ISearchConsoleMetricaRepository _metricaRepository;
    private readonly ISearchConsoleOAuthRepository _oauthRepository;
    private readonly ILogger<SincronizarSearchConsolePorUsuarioCommandHandler> _logger;

    public SincronizarSearchConsolePorUsuarioCommandHandler(
        IBlogRepository blogRepository,
        IBlogDominioRepository blogDominioRepository,
        ISearchConsoleClient searchConsoleClient,
        ISearchConsoleMetricaRepository metricaRepository,
        ISearchConsoleOAuthRepository oauthRepository,
        ILogger<SincronizarSearchConsolePorUsuarioCommandHandler> logger)
    {
        _blogRepository = blogRepository;
        _blogDominioRepository = blogDominioRepository;
        _searchConsoleClient = searchConsoleClient;
        _metricaRepository = metricaRepository;
        _oauthRepository = oauthRepository;
        _logger = logger;
    }

    public async Task<SincronizarSearchConsoleResult> Handle(SincronizarSearchConsolePorUsuarioCommand request, CancellationToken cancellationToken)
    {
        var oauth = await _oauthRepository.ObterPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
        if (oauth == null)
        {
            _logger.LogWarning("Sincronização Search Console por usuário: usuário {UsuarioId} não possui GSC conectado.", request.UsuarioId);
            return new SincronizarSearchConsoleResult(0, 0, 0);
        }

        var blogs = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        var blogIds = blogs.Select(b => b.Id).ToList();
        var primeiraSincronizacao = blogIds.Count > 0 && !await _metricaRepository.ExisteAlgumaMetricaParaBlogsAsync(blogIds, cancellationToken);

        var dataPadrao = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-DiasAtrasPadrao));
        var datasParaSincronizar = ObterDatasParaSincronizar(request.DataAlvo, dataPadrao, primeiraSincronizacao);

        var dominiosProcessados = 0;
        var metricasSalvas = 0;
        var falhas = 0;

        foreach (var dataAlvo in datasParaSincronizar)
        {
            foreach (var blog in blogs)
            {
                var dominios = await _blogDominioRepository.ListarPorBlogAsync(blog.Id, cancellationToken);
                foreach (var dominio in dominios)
                {
                    dominiosProcessados++;
                    SearchConsoleMetricasDto? dto;
                    try
                    {
                        dto = await _searchConsoleClient.ObterMetricasAgregadasAsync(
                            dominio.NomeDominio,
                            dataAlvo,
                            TipoBuscaPadrao,
                            oauth.RefreshToken,
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        falhas++;
                        _logger.LogWarning(ex,
                            "Sincronização Search Console por usuário: falha ao obter métricas para domínio {NomeDominio} em {Data} (usuário {UsuarioId}). Motivo: {Motivo}",
                            dominio.NomeDominio, dataAlvo, request.UsuarioId, ex.Message);
                        continue;
                    }

                    if (dto == null)
                    {
                        falhas++;
                        _logger.LogWarning(
                            "Sincronização Search Console por usuário: falha ao obter métricas para domínio {NomeDominio} em {Data} (usuário {UsuarioId}) — sem dados.",
                            dominio.NomeDominio, dataAlvo, request.UsuarioId);
                        continue;
                    }

                    try
                    {
                        var metrica = new SearchConsoleMetrica
                        {
                            Id = Guid.NewGuid(),
                            BlogDominioId = dominio.Id,
                            Data = dataAlvo,
                            TipoBusca = TipoBuscaPadrao,
                            Impressoes = dto.Impressoes,
                            Cliques = dto.Cliques,
                            Ctr = dto.Ctr,
                            PosicaoMedia = dto.PosicaoMedia,
                            DataSincronizacao = DateTime.UtcNow
                        };
                        await _metricaRepository.InserirOuAtualizarAsync(metrica, cancellationToken);
                        metricasSalvas++;
                    }
                    catch (Exception ex)
                    {
                        falhas++;
                        _logger.LogWarning(ex,
                            "Sincronização Search Console por usuário: falha ao salvar métricas para domínio {NomeDominio} em {Data}. Seguindo para o próximo. Motivo: {Motivo}",
                            dominio.NomeDominio, dataAlvo, ex.Message);
                    }
                }
            }
        }

        _logger.LogInformation(
            "Sincronização Search Console (usuário {UsuarioId}): primeiraVez={PrimeiraVez}, datas={Datas}, domínios processados={Dominios}, métricas salvas={Metricas}, falhas={Falhas}.",
            request.UsuarioId, primeiraSincronizacao, datasParaSincronizar.Count, dominiosProcessados, metricasSalvas, falhas);

        return new SincronizarSearchConsoleResult(dominiosProcessados, metricasSalvas, falhas);
    }

    /// <summary>
    /// Retorna as datas a sincronizar: uma única data (3 dias atrás) ou os últimos 90 dias na primeira sincronização.
    /// </summary>
    private static IReadOnlyList<DateOnly> ObterDatasParaSincronizar(DateOnly? dataAlvoExplicita, DateOnly dataPadrao, bool primeiraSincronizacao)
    {
        if (dataAlvoExplicita.HasValue)
            return new[] { dataAlvoExplicita.Value };

        if (!primeiraSincronizacao)
            return new[] { dataPadrao };

        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var dataInicio = hoje.AddDays(-PrimeiraSincronizacaoDias);
        var dataFim = hoje.AddDays(-DiasAtrasPadrao);
        var lista = new List<DateOnly>();
        for (var d = dataInicio; d <= dataFim; d = d.AddDays(1))
            lista.Add(d);
        return lista;
    }
}
