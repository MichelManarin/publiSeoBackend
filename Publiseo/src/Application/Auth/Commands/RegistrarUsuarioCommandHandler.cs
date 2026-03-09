using Application.Auth.Abstractions;
using Application.Auth.Contracts;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Auth.Commands;

[ExcludeFromCodeCoverage]
public sealed class RegistrarUsuarioCommandHandler : IRequestHandler<RegistrarUsuarioCommand, RegistrarUsuarioResponse?>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegistrarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegistrarUsuarioResponse?> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existentePorLogin = await _usuarioRepository.ObterPorLoginAsync(request.Login.Trim(), cancellationToken);
        if (existentePorLogin != null)
            return null;

        var emailNormalizado = request.Email.Trim().ToLowerInvariant();
        var existentePorEmail = await _usuarioRepository.ObterPorEmailAsync(emailNormalizado, cancellationToken);
        if (existentePorEmail != null)
            return null;

        var pais = string.IsNullOrWhiteSpace(request.Pais) ? "Brasil" : request.Pais.Trim();

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome.Trim(),
            Sobrenome = request.Sobrenome.Trim(),
            Email = emailNormalizado,
            Telefone = request.Telefone.Trim(),
            Login = request.Login.Trim().ToLowerInvariant(),
            SenhaHash = _passwordHasher.Hash(request.Senha),
            Endereco = string.IsNullOrWhiteSpace(request.Endereco) ? null : request.Endereco.Trim(),
            Cidade = string.IsNullOrWhiteSpace(request.Cidade) ? null : request.Cidade.Trim(),
            Estado = string.IsNullOrWhiteSpace(request.Estado) ? null : request.Estado.Trim().ToUpperInvariant(),
            CodigoPostal = string.IsNullOrWhiteSpace(request.CodigoPostal) ? null : request.CodigoPostal.Trim(),
            Pais = pais,
            DataCriacao = DateTime.UtcNow
        };
        await _usuarioRepository.InserirAsync(usuario, cancellationToken);

        var nomeCompleto = $"{usuario.Nome} {usuario.Sobrenome}".Trim();
        return new RegistrarUsuarioResponse(usuario.Id, nomeCompleto, usuario.Email, usuario.Login);
    }
}
