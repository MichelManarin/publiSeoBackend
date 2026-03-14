using Application.SearchConsole.Contracts;
using MediatR;

namespace Application.SearchConsole.Queries;

/// <summary>
/// Retorna se o usuário possui Google Search Console conectado (OAuth) e o e-mail da conta.
/// </summary>
public record ObterSearchConsoleStatusQuery(Guid UsuarioId) : IRequest<SearchConsoleStatusResponse>;
