using Application.Artigo.Contracts;
using MediatR;

namespace Application.Artigo.Commands;

/// <summary>
/// Processa artigos com geração por IA pendente (status Pendente, tentativas abaixo do máximo).
/// Pode ser disparado manualmente ou pelo job Quartz.
/// </summary>
public record ProcessarArtigosPendentesCommand : IRequest<ProcessarArtigosPendentesResult>;
