namespace Application.Auth.Contracts;

public record LoginResponse(string AccessToken, Guid UsuarioId, string NomeCompleto, string Login);
