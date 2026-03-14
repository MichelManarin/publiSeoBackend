using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Commands;

[ExcludeFromCodeCoverage]
public sealed class DesvincularSearchConsoleCommandHandler : IRequestHandler<DesvincularSearchConsoleCommand, Unit>
{
    private readonly ISearchConsoleOAuthRepository _oauthRepository;

    public DesvincularSearchConsoleCommandHandler(ISearchConsoleOAuthRepository oauthRepository)
    {
        _oauthRepository = oauthRepository;
    }

    public async Task<Unit> Handle(DesvincularSearchConsoleCommand request, CancellationToken cancellationToken)
    {
        await _oauthRepository.RemoverPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
        return Unit.Value;
    }
}
