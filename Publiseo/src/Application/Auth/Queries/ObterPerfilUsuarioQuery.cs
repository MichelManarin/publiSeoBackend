using Application.Auth.Contracts;
using MediatR;

namespace Application.Auth.Queries;

/// <summary>Obtém o perfil do usuário autenticado (id do token via ICurrentUserIdProvider).</summary>
public record ObterPerfilUsuarioQuery : IRequest<PerfilUsuarioResponse?>;
