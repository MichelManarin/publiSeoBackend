using Application.SearchConsole.Contracts;
using MediatR;

namespace Application.SearchConsole.Commands;

/// <summary>
/// Sincroniza métricas do Google Search Console apenas para os domínios do usuário informado.
/// Usado após conectar a conta (para não esperar o job) ou pelo endpoint "sincronizar-me".
/// </summary>
public record SincronizarSearchConsolePorUsuarioCommand(
    Guid UsuarioId,
    DateOnly? DataAlvo = null) : IRequest<SincronizarSearchConsoleResult>;
