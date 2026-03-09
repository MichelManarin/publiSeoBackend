using Application.Dominio.Adapters;
using Application.Dominio.Contracts;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dominio.Queries;

[ExcludeFromCodeCoverage]
public sealed class SugerirDominiosQueryHandler : IRequestHandler<SugerirDominiosQuery, IReadOnlyList<SugestaoDominioItemResponse>>
{
    private readonly IDominioAdapter _adapter;

    public SugerirDominiosQueryHandler(IDominioAdapter adapter) => _adapter = adapter;

    public Task<IReadOnlyList<SugestaoDominioItemResponse>> Handle(SugerirDominiosQuery request, CancellationToken cancellationToken)
    {
        var options = new SugestaoDominioOptions
        {
            Query = request.Query,
            Country = request.Country,
            City = request.City,
            Sources = request.Sources,
            Tlds = request.Tlds,
            LengthMin = request.LengthMin,
            LengthMax = request.LengthMax,
            Limit = request.Limit,
            WaitMs = request.WaitMs
        };
        return _adapter.SugerirAsync(options, request.ShopperId, cancellationToken);
    }
}
