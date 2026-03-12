using Application.Artigo.Abstractions;
using Application.Artigo.Contracts;
using Application.Artigo.Options;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Application.Artigo.Commands;

[ExcludeFromCodeCoverage]
public sealed class ProcessarArtigosPendentesCommandHandler : IRequestHandler<ProcessarArtigosPendentesCommand, ProcessarArtigosPendentesResult>
{
    private readonly IArtigoRepository _artigoRepository;
    private readonly IGeradorConteudoArtigoService _geradorConteudo;
    private readonly ArtigoGeracaoOptions _opcoes;
    private readonly ILogger<ProcessarArtigosPendentesCommandHandler> _logger;

    public ProcessarArtigosPendentesCommandHandler(
        IArtigoRepository artigoRepository,
        IGeradorConteudoArtigoService geradorConteudo,
        IOptions<ArtigoGeracaoOptions> opcoes,
        ILogger<ProcessarArtigosPendentesCommandHandler> logger)
    {
        _artigoRepository = artigoRepository;
        _geradorConteudo = geradorConteudo;
        _opcoes = opcoes.Value;
        _logger = logger;
    }

    public async Task<ProcessarArtigosPendentesResult> Handle(ProcessarArtigosPendentesCommand request, CancellationToken cancellationToken)
    {
        var pendentes = await _artigoRepository.ListarPendentesGeracaoAsync(_opcoes.MaxTentativas, cancellationToken);
        var processados = 0;
        var sucesso = 0;
        var falha = 0;

        foreach (var artigo in pendentes)
        {
            processados++;
            artigo.StatusGeracao = StatusGeracaoArtigo.EmProcessamento;
            artigo.DataAtualizacao = DateTime.UtcNow;
            await _artigoRepository.AtualizarAsync(artigo, cancellationToken);

            var numeroPalavras = _opcoes.NumeroPalavrasDefault > 0 ? _opcoes.NumeroPalavrasDefault : 500;
            var conteudo = await _geradorConteudo.GerarConteudoAsync(artigo.Titulo, numeroPalavras, cancellationToken);

            if (!string.IsNullOrWhiteSpace(conteudo))
            {
                artigo.Conteudo = conteudo.Trim();
                artigo.StatusGeracao = StatusGeracaoArtigo.Concluido;
                artigo.DataAtualizacao = DateTime.UtcNow;
                sucesso++;
                _logger.LogInformation("Artigo {ArtigoId} gerado com sucesso por IA.", artigo.Id);
            }
            else
            {
                artigo.TentativasGeracao++;
                artigo.StatusGeracao = artigo.TentativasGeracao >= _opcoes.MaxTentativas
                    ? StatusGeracaoArtigo.Falha
                    : StatusGeracaoArtigo.Pendente;
                artigo.DataAtualizacao = DateTime.UtcNow;
                falha++;
                _logger.LogWarning("Falha na geração por IA do artigo {ArtigoId}. Tentativa {Tentativa}/{MaxTentativas}.",
                    artigo.Id, artigo.TentativasGeracao, _opcoes.MaxTentativas);
            }

            await _artigoRepository.AtualizarAsync(artigo, cancellationToken);
        }

        return new ProcessarArtigosPendentesResult(processados, sucesso, falha);
    }
}
