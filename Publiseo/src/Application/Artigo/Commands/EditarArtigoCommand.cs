using Application.Artigo.Contracts;
using MediatR;

namespace Application.Artigo.Commands;

public record EditarArtigoCommand(
    Guid Id,
    Guid UsuarioId,
    string Titulo,
    string? MetaDescription,
    string Conteudo,
    Domain.Enums.TipoRascunho TipoRascunho) : IRequest<ArtigoResponse?>;
