using Application.Dominio.Contracts;
using MediatR;

namespace Application.Dominio.Queries;

public record VerificarDisponibilidadeDominioQuery(string Dominio) : IRequest<DominioDisponibilidadeResponse>;
