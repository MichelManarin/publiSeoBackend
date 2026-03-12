namespace Application.Auth.Contracts;

public record EditarUsuarioRequest(
    string Nome,
    string Sobrenome,
    string Email,
    string Telefone,
    string? Endereco = null,
    string? Cidade = null,
    string? Estado = null,
    string? CodigoPostal = null,
    string? Pais = null);
