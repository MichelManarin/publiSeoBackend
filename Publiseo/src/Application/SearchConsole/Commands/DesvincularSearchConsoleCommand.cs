using MediatR;

namespace Application.SearchConsole.Commands;

/// <summary>
/// Remove o vínculo OAuth do usuário com o Google Search Console.
/// </summary>
public record DesvincularSearchConsoleCommand(Guid UsuarioId) : IRequest<Unit>;
