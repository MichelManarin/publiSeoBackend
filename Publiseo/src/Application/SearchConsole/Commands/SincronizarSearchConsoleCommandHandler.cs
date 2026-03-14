using Application.SearchConsole.Abstractions;
using Application.SearchConsole.Contracts;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Commands;

[ExcludeFromCodeCoverage]
public sealed class SincronizarSearchConsoleCommandHandler : IRequestHandler<SincronizarSearchConsoleCommand, SincronizarSearchConsoleResult>
{
    private const string TipoBuscaPadrao = "web";
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogDominioRepository _blogDominioRepository;
    private readonly ISearchConsoleClient _searchConsoleClient;
    private readonly ISearchConsoleMetricaRepository _metricaRepository;
    private readonly ISearchConsoleOAuthRepository _oauthRepository;
    private readonly ILogger<SincronizarSearchConsoleCommandHandler> _logger;

    public SincronizarSearchConsoleCommandHandler(
        IBlogRepository blogRepository,
        IBlogDominioRepository blogDominioRepository,
        ISearchConsoleClient searchConsoleClient,
        ISearchConsoleMetricaRepository metricaRepository,
        ISearchConsoleOAuthRepository oauthRepository,
        ILogger<SincronizarSearchConsoleCommandHandler> logger)
    {
        _blogRepository = blogRepository;
        _blogDominioRepository = blogDominioRepository;
        _searchConsoleClient = searchConsoleClient;
        _metricaRepository = metricaRepository;
        _oauthRepository = oauthRepository;
        _logger = logger;
    }

    public async Task<SincronizarSearchConsoleResult> Handle(SincronizarSearchConsoleCommand request, CancellationToken cancellationToken)
    {
        var dataAlvo = request.DataAlvo ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var usuariosOAuth = await _oauthRepository.ListarTodosComTokenAsync(cancellationToken);
        var dominiosProcessados = 0;
        var metricasSalvas = 0;
        var falhas = 0;

        foreach (var oauth in usuariosOAuth)
        {
            var blogs = await _blogRepository.ListarPorUsuarioAsync(oauth.UsuarioId, cancellationToken);
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
        }

        _logger.LogInformation(
            "Sincronização Search Console: data={Data}, domínios={Dominios}, métricas salvas={Metricas}, falhas={Falhas}.",
            dataAlvo, dominiosProcessados, metricasSalvas, falhas);

        return new SincronizarSearchConsoleResult(dominiosProcessados, metricasSalvas, falhas);
    }
}
