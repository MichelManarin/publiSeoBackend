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

    public VincularSearchConsoleOAuthCommandHandler(
        IGoogleSearchConsoleOAuthService oauthService,
        ISearchConsoleOAuthRepository oauthRepository)
    {
        _oauthService = oauthService;
        _oauthRepository = oauthRepository;
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
            return true;
        }
        catch
        {
            return false;
        }
    }
}
