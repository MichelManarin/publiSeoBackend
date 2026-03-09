using Application.Auth.Contracts;
using MediatR;

namespace Application.Auth.Commands;

public record RegistrarUsuarioCommand(
    string Nome,
    string Sobrenome,
    string Email,
    string Telefone,
    string Login,
    string Senha,
    string? Endereco = null,
    string? Cidade = null,
    string? Estado = null,
    string? CodigoPostal = null,
    string? Pais = null) : IRequest<RegistrarUsuarioResponse?>;
