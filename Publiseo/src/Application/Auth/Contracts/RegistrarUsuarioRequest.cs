namespace Application.Auth.Contracts;

public record RegistrarUsuarioRequest(
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
    string? Pais = null);
