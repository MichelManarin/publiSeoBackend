using Application.Abstractions;
using System.Security.Claims;

namespace API.Services;

/// <summary>
/// Obtém o id do usuário autenticado a partir do JWT (claim "sub" ou NameIdentifier).
/// </summary>
public sealed class CurrentUserIdProvider : ICurrentUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return null;
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}
