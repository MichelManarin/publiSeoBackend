using Application.Dominio.Adapters;
using Application.Dominio.Contracts;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dominio.Queries;

[ExcludeFromCodeCoverage]
public sealed class VerificarDisponibilidadeDominioQueryHandler : IRequestHandler<VerificarDisponibilidadeDominioQuery, DominioDisponibilidadeResponse>
{
    private readonly IDominioAdapter _adapter;

    public VerificarDisponibilidadeDominioQueryHandler(IDominioAdapter adapter) => _adapter = adapter;

    public Task<DominioDisponibilidadeResponse> Handle(VerificarDisponibilidadeDominioQuery request, CancellationToken cancellationToken)
        => _adapter.VerificarDisponibilidadeAsync(request.Dominio, cancellationToken);
}
