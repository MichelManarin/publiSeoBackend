using MediatR;

namespace Application.Abstractions;

/// <summary>
/// Abstração do mediator para envio de commands e queries.
/// A API depende apenas desta interface; o pacote MediatR fica restrito à Application.
/// </summary>
public interface IApplicationMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
