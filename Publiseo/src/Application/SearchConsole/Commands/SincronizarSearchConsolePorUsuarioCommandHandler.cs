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
        var dataAlvo = request.DataAlvo ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var oauth = await _oauthRepository.ObterPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
        if (oauth == null)
        {
            _logger.LogWarning("Sincronização Search Console por usuário: usuário {UsuarioId} não possui GSC conectado.", request.UsuarioId);
            return new SincronizarSearchConsoleResult(0, 0, 0);
        }

        var dominiosProcessados = 0;
        var metricasSalvas = 0;
        var falhas = 0;

        var blogs = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        foreach (var blog in blogs)
        {
            var dominios = await _blogDominioRepository.ListarPorBlogAsync(blog.Id, cancellationToken);
            foreach (var dominio in dominios)
            {
                dominiosProcessados++;
                var dto = await _searchConsoleClient.ObterMetricasAgregadasAsync(
                    dominio.NomeDominio,
                    dataAlvo,
                    TipoBuscaPadrao,
                    oauth.RefreshToken,
                    cancellationToken);

                if (dto == null)
                {
                    falhas++;
                    continue;
                }

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
        }

        _logger.LogInformation(
            "Sincronização Search Console (usuário {UsuarioId}): data={Data}, domínios={Dominios}, métricas salvas={Metricas}, falhas={Falhas}.",
            request.UsuarioId, dataAlvo, dominiosProcessados, metricasSalvas, falhas);

        return new SincronizarSearchConsoleResult(dominiosProcessados, metricasSalvas, falhas);
    }
}
