using Application.Auth.Contracts;
using MediatR;

namespace Application.Auth.Commands;

public record LoginCommand(string Login, string Senha, string? IpRemoto = null) : IRequest<LoginResponse?>;
