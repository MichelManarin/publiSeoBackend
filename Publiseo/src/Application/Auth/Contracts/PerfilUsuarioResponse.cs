namespace Application.Auth.Contracts;

/// <summary>Dados do perfil do usuário autenticado (para GET /api/usuario).</summary>
public record PerfilUsuarioResponse(
    Guid UsuarioId,
    string NomeCompleto,
    string Email,
    string Login,
    string Telefone,
    string? Endereco,
    string? Cidade,
    string? Estado,
    string? CodigoPostal,
    string? Pais);
