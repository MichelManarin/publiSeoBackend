using Application.Auth.Abstractions;
using Application.Auth.Contracts;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Auth.Commands;

[ExcludeFromCodeCoverage]
public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorLoginAsync(request.Login, cancellationToken);
        if (usuario == null || !_passwordHasher.Verify(request.Senha, usuario.SenhaHash))
            return null;

        var token = _jwtTokenService.GenerateAccessToken(usuario.Id, usuario.Login);
        usuario.UltimoLogin = DateTime.UtcNow;
        usuario.UltimoTokenGerado = token;
        usuario.UltimoIp = request.IpRemoto;
        await _usuarioRepository.AtualizarAsync(usuario, cancellationToken);

        var nomeCompleto = $"{usuario.Nome} {usuario.Sobrenome}".Trim();
        return new LoginResponse(token, usuario.Id, nomeCompleto, usuario.Login);
    }
}
