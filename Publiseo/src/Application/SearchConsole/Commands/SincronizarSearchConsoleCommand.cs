using Application.SearchConsole.Contracts;
using MediatR;

namespace Application.SearchConsole.Commands;

/// <summary>
/// Sincroniza métricas do Google Search Console para todos os domínios (um dia de dados).
/// Pode ser disparado pelo job diário ou manualmente.
/// </summary>
public record SincronizarSearchConsoleCommand(DateOnly? DataAlvo = null) : IRequest<SincronizarSearchConsoleResult>;
