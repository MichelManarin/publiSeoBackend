using MediatR;

namespace Application.SearchConsole.Commands;

/// <summary>
/// Vincula a conta Google Search Console ao usuário (troca o code OAuth por refresh token e persiste).
/// </summary>
public record VincularSearchConsoleOAuthCommand(
    Guid UsuarioId,
    string Code,
    string RedirectUri) : IRequest<bool>;
