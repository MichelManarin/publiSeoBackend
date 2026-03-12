using Application.Abstractions;
using Application.Auth.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Auth.Queries;

[ExcludeFromCodeCoverage]
public sealed class ObterPerfilUsuarioQueryHandler : IRequestHandler<ObterPerfilUsuarioQuery, PerfilUsuarioResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public ObterPerfilUsuarioQueryHandler(IUsuarioRepository usuarioRepository, ICurrentUserIdProvider currentUserIdProvider)
    {
        _usuarioRepository = usuarioRepository;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<PerfilUsuarioResponse?> Handle(ObterPerfilUsuarioQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUserIdProvider.GetCurrentUserId();
        if (usuarioId == null)
            return null;
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId.Value, cancellationToken);
        if (usuario == null)
            return null;
        var nomeCompleto = $"{usuario.Nome} {usuario.Sobrenome}".Trim();
        return new PerfilUsuarioResponse(
            usuario.Id,
            nomeCompleto,
            usuario.Email,
            usuario.Login,
            usuario.Telefone,
            usuario.Endereco,
            usuario.Cidade,
            usuario.Estado,
            usuario.CodigoPostal,
            usuario.Pais);
    }
}
