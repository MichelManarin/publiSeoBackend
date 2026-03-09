namespace Application.Auth.Contracts;

public record RegistrarUsuarioResponse(Guid UsuarioId, string NomeCompleto, string Email, string Login);
