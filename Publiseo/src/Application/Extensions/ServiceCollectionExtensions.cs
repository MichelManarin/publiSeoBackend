using Application.Abstractions;
using Application.Behaviors;
using Application.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(Application.AssemblyReference).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddScoped<IApplicationMediator, ApplicationMediatorAdapter>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        // TransactionBehavior removido: não é compatível com NpgsqlRetryingExecutionStrategy (EnableRetryOnFailure).
        // Cada handler usa o DbContext scoped; SaveChangesAsync já forma uma transação por operação.

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
