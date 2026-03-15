using Application.Conversor.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Conversor.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterConversorPublicoPorBlogQueryHandler : IRequestHandler<ObterConversorPublicoPorBlogQuery, ConversorPublicoResponse?>
{
    private readonly IConversorRepository _conversorRepository;

    public ObterConversorPublicoPorBlogQueryHandler(IConversorRepository conversorRepository)
        => _conversorRepository = conversorRepository;

    public async Task<ConversorPublicoResponse?> Handle(ObterConversorPublicoPorBlogQuery request, CancellationToken cancellationToken)
    {
        var conversor = await _conversorRepository.ObterPorBlogExternalIdAsync(request.BlogExternalId, cancellationToken);
        if (conversor == null || !conversor.Ativo)
            return null;

        var ordenadas = conversor.Perguntas.OrderBy(p => p.Ordem).ToList();
        return new ConversorPublicoResponse(
            conversor.Ativo,
            conversor.TextoBotaoInicial,
            conversor.TipoFinalizacao,
            conversor.MensagemFinalizacao,
            conversor.WhatsAppNumero,
            conversor.WhatsAppTextoPreDefinido,
            ordenadas.Select(p => new ConversorPerguntaPublicoItemDto(p.Ordem, p.TextoPergunta, p.TipoCampo)).ToList());
    }
}
