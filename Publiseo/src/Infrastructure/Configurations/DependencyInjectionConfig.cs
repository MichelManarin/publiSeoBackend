using Application.Auth.Abstractions;
using Application.Data;
using Application.Dominio.Adapters;
using Application.Dominio.Contracts;
using Domain.Interfaces.Repositories;
using Infrastructure.Auth;
using Infrastructure.Context;
using Infrastructure.Data;
using Infrastructure.GoDaddy;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations;

public static class DependencyInjectionConfig
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var raw = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Database=publiseo;Username=postgres;Password=postgres";
        var connectionString = ToNpgsqlConnectionString(raw);

        services.AddDbContext<PubliseoDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
                npgsql.CommandTimeout(30);
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });

        services.AddScoped<IComplexQueryExecutor, ComplexQueryExecutor>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IBlogMembroRepository, BlogMembroRepository>();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.Configure<GoDaddyOptions>(configuration.GetSection(GoDaddyOptions.SectionName));
        services.Configure<CompanyDomainSettings>(configuration.GetSection("DominioCompra:CompanyContact"));
        services.AddHttpClient<GoDaddyDominioAdapter>();
        services.AddTransient<IDominioAdapter>(sp => sp.GetRequiredService<GoDaddyDominioAdapter>());

        services.AddScoped<IDominioRepository, DominioRepository>();

        return services;
    }

    private static string ToNpgsqlConnectionString(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.TrimStart().StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            return value;
        try
        {
            var uri = new Uri(value);
            var user = uri.UserInfo?.Split(':', 2) ?? [];
            var userPart = user.Length > 0 ? Uri.UnescapeDataString(user[0]) : "postgres";
            var passPart = user.Length > 1 ? Uri.UnescapeDataString(user[1]) : "";
            var port = uri.Port > 0 ? uri.Port : 5432;
            var db = string.IsNullOrEmpty(uri.AbsolutePath) || uri.AbsolutePath == "/" ? "postgres" : uri.AbsolutePath.TrimStart('/');
            return $"Host={uri.Host};Port={port};Database={db};Username={userPart};Password={passPart};SSL Mode=Require;Trust Server Certificate=true";
        }
        catch
        {
            return value;
        }
    }
}
