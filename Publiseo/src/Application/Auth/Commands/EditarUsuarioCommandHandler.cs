using Application.Abstractions;
using Application.Auth.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Auth.Commands;

[ExcludeFromCodeCoverage]
public sealed class EditarUsuarioCommandHandler : IRequestHandler<EditarUsuarioCommand, EditarUsuarioResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public EditarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, ICurrentUserIdProvider currentUserIdProvider)
    {
        _usuarioRepository = usuarioRepository;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<EditarUsuarioResponse?> Handle(EditarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUserIdProvider.GetCurrentUserId();
        if (usuarioId == null)
            return null;
        var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId.Value, cancellationToken);
        if (usuario == null)
            return null;

        var emailNormalizado = request.Email.Trim().ToLowerInvariant();
        if (usuario.Email != emailNormalizado)
        {
            var existentePorEmail = await _usuarioRepository.ObterPorEmailAsync(emailNormalizado, cancellationToken);
            if (existentePorEmail != null)
                return null;
        }

        usuario.Nome = request.Nome.Trim();
        usuario.Sobrenome = request.Sobrenome.Trim();
        usuario.Email = emailNormalizado;
        usuario.Telefone = request.Telefone.Trim();
        usuario.Endereco = string.IsNullOrWhiteSpace(request.Endereco) ? null : request.Endereco.Trim();
        usuario.Cidade = string.IsNullOrWhiteSpace(request.Cidade) ? null : request.Cidade.Trim();
        usuario.Estado = string.IsNullOrWhiteSpace(request.Estado) ? null : request.Estado.Trim().ToUpperInvariant();
        usuario.CodigoPostal = string.IsNullOrWhiteSpace(request.CodigoPostal) ? null : request.CodigoPostal.Trim();
        usuario.Pais = string.IsNullOrWhiteSpace(request.Pais) ? "Brasil" : request.Pais.Trim();

        await _usuarioRepository.AtualizarAsync(usuario, cancellationToken);

        var nomeCompleto = $"{usuario.Nome} {usuario.Sobrenome}".Trim();
        return new EditarUsuarioResponse(usuario.Id, nomeCompleto, usuario.Email, usuario.Login);
    }
}
