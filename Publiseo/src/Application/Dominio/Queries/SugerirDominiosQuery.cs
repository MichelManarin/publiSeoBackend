using Application.Dominio.Contracts;
using MediatR;

namespace Application.Dominio.Queries;

public record SugerirDominiosQuery(
    string Query,
    string? Country = null,
    string? City = null,
    IReadOnlyList<string>? Sources = null,
    IReadOnlyList<string>? Tlds = null,
    int? LengthMin = null,
    int? LengthMax = null,
    int? Limit = null,
    int? WaitMs = null,
    string? ShopperId = null
) : IRequest<IReadOnlyList<SugestaoDominioItemResponse>>;
