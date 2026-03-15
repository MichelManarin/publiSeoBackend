using Application.Conversor.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Conversor.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterConversorPorBlogQueryHandler : IRequestHandler<ObterConversorPorBlogQuery, ConversorConfigResponse?>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IConversorRepository _conversorRepository;

    public ObterConversorPorBlogQueryHandler(
        IBlogRepository blogRepository,
        IConversorRepository conversorRepository)
    {
        _blogRepository = blogRepository;
        _conversorRepository = conversorRepository;
    }

    public async Task<ConversorConfigResponse?> Handle(ObterConversorPorBlogQuery request, CancellationToken cancellationToken)
    {
        var blogsDoUsuario = await _blogRepository.ListarPorUsuarioAsync(request.UsuarioId, cancellationToken);
        if (blogsDoUsuario.All(b => b.Id != request.BlogId))
            return null;

        var conversor = await _conversorRepository.ObterPorBlogIdComPerguntasAsync(request.BlogId, cancellationToken);
        if (conversor == null)
            return null;

        var ordenadas = conversor.Perguntas.OrderBy(p => p.Ordem).ToList();
        return new ConversorConfigResponse(
            conversor.Id,
            conversor.BlogId,
            conversor.Ativo,
            conversor.TextoBotaoInicial,
            conversor.TipoFinalizacao,
            conversor.MensagemFinalizacao,
            conversor.WhatsAppNumero,
            conversor.WhatsAppTextoPreDefinido,
            ordenadas.Select(p => new ConversorPerguntaItemDto(p.Id, p.Ordem, p.TextoPergunta, p.TipoCampo)).ToList(),
            conversor.DataCriacao,
            conversor.DataAtualizacao);
    }
}
