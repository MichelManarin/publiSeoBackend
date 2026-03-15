using Application.Conversor.Contracts;
using MediatR;

namespace Application.Conversor.Commands;

public record SalvarConversorCommand(
    Guid UsuarioId,
    Guid BlogId,
    SalvarConversorRequest Request) : IRequest<ConversorConfigResponse?>;
