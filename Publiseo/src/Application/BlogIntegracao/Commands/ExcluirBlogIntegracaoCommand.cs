using MediatR;

namespace Application.BlogIntegracao.Commands;

public record ExcluirBlogIntegracaoCommand(Guid UsuarioId, Guid BlogId, Guid IntegracaoId) : IRequest<bool>;
