using Application.BlogIntegracao.Contracts;
using MediatR;

namespace Application.BlogIntegracao.Commands;

public record AtualizarBlogIntegracaoCommand(
    Guid UsuarioId,
    Guid BlogId,
    Guid IntegracaoId,
    string? Tipo,
    string? Valor,
    int? Ordem) : IRequest<BlogIntegracaoItemDto?>;
