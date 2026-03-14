using Application.SearchConsole.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.SearchConsole.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterSearchConsoleStatusQueryHandler : IRequestHandler<ObterSearchConsoleStatusQuery, SearchConsoleStatusResponse>
{
    private readonly ISearchConsoleOAuthRepository _oauthRepository;

    public ObterSearchConsoleStatusQueryHandler(ISearchConsoleOAuthRepository oauthRepository)
    {
        _oauthRepository = oauthRepository;
    }

    public async Task<SearchConsoleStatusResponse> Handle(ObterSearchConsoleStatusQuery request, CancellationToken cancellationToken)
    {
        var oauth = await _oauthRepository.ObterPorUsuarioIdAsync(request.UsuarioId, cancellationToken);
        return new SearchConsoleStatusResponse(oauth != null, oauth?.EmailGoogle);
    }
}
