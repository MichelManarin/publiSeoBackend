namespace Application.Abstractions;

/// <summary>
/// Fornece o id do usuário autenticado (ex.: do JWT). A implementação fica na API/camada de apresentação.
/// </summary>
public interface ICurrentUserIdProvider
{
    Guid? GetCurrentUserId();
}
