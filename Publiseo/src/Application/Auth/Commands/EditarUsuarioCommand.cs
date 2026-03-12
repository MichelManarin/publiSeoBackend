using Application.Auth.Contracts;
using MediatR;

namespace Application.Auth.Commands;

/// <summary>
/// Edita o perfil do usuário autenticado. O id é obtido do token via ICurrentUserIdProvider.
/// </summary>
public record EditarUsuarioCommand(
    string Nome,
    string Sobrenome,
    string Email,
    string Telefone,
    string? Endereco = null,
    string? Cidade = null,
    string? Estado = null,
    string? CodigoPostal = null,
    string? Pais = null) : IRequest<EditarUsuarioResponse?>;
