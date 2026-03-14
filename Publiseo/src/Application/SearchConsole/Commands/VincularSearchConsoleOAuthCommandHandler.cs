using Application.SearchConsole.Abstractions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Commands;

[ExcludeFromCodeCoverage]
public sealed class VincularSearchConsoleOAuthCommandHandler : IRequestHandler<VincularSearchConsoleOAuthCommand, bool>
{
    private readonly IGoogleSearchConsoleOAuthService _oauthService;
    private readonly ISearchConsoleOAuthRepository _oauthRepository;
    private readonly IMediator _mediator;

    public VincularSearchConsoleOAuthCommandHandler(
        IGoogleSearchConsoleOAuthService oauthService,
        ISearchConsoleOAuthRepository oauthRepository,
        IMediator mediator)
    {
        _oauthService = oauthService;
        _oauthRepository = oauthRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(VincularSearchConsoleOAuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (refreshToken, email) = await _oauthService.ExchangeCodeForTokenAsync(
                request.Code,
                request.RedirectUri,
                cancellationToken);

            var oauth = new SearchConsoleOAuth
            {
                Id = Guid.NewGuid(),
                UsuarioId = request.UsuarioId,
                RefreshToken = refreshToken,
                EmailGoogle = email,
                DataVinculo = DateTime.UtcNow
            };
            await _oauthRepository.InserirOuAtualizarAsync(oauth, cancellationToken);

            await _mediator.Send(new SincronizarSearchConsolePorUsuarioCommand(request.UsuarioId), cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
