namespace Application.Auth.Contracts;

public record EditarUsuarioResponse(Guid UsuarioId, string NomeCompleto, string Email, string Login);
