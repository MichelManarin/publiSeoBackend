using Application.Abstractions;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Infrastructure;

[ExcludeFromCodeCoverage]
internal sealed class ApplicationMediatorAdapter : IApplicationMediator
{
    private readonly IMediator _mediator;

    public ApplicationMediatorAdapter(IMediator mediator) => _mediator = mediator;

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => _mediator.Send(request, cancellationToken);
}
