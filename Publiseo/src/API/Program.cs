using System.Text;
using API.Configuration;
using API.Jobs;
using API.Middleware;
using API.Services;
using Application.Abstractions;
using Application.Extensions;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserIdProvider, CurrentUserIdProvider>();
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

var configuration = builder.Configuration;

builder.Services.AddApiConfig(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApplication();

var jwtSecret = configuration["Jwt:Secret"] ?? "";
var jwtKey = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(jwtSecret) ? "CHANGE_ME_MIN_32_CHARS_FOR_HS256!" : jwtSecret);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"] ?? "Publiseo",
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"] ?? "Publiseo",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("ProcessarArtigosPendentes");
    q.AddJob<ProcessarArtigosPendentesJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ProcessarArtigosPendentes-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()));

    var gscJobKey = new JobKey("SincronizarSearchConsole");
    q.AddJob<SincronizarSearchConsoleJob>(opts => opts.WithIdentity(gscJobKey));
    q.AddTrigger(opts => opts
        .ForJob(gscJobKey)
        .WithIdentity("SincronizarSearchConsole-trigger")
        .WithCronSchedule("0 0 4 * * ?")); // Todo dia às 4h da manhã (UTC)
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
// Permite injetar IScheduler no ArtigoController (disparo manual do job).
builder.Services.AddScoped<IScheduler>(sp =>
{
    var factory = sp.GetRequiredService<ISchedulerFactory>();
    return factory.GetScheduler().GetAwaiter().GetResult();
});

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
    app.Urls.Add($"http://0.0.0.0:{port}");

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseApiConfig(app.Environment);
app.MapControllers();

await app.RunAsync();
