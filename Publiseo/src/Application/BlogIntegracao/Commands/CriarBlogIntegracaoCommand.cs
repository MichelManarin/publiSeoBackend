using Application.BlogIntegracao.Contracts;
using MediatR;

namespace Application.BlogIntegracao.Commands;

public record CriarBlogIntegracaoCommand(
    Guid UsuarioId,
    Guid BlogId,
    string Tipo,
    string Valor,
    int Ordem = 0) : IRequest<BlogIntegracaoItemDto?>;
