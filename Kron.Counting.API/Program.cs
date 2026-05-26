using AspNetCoreRateLimit;
using FluentMigrator.Runner;
using Kron.Counting.API.Middleware;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Infrastructure.Data;
using Kron.Counting.Infrastructure.Migrations;
using Kron.Counting.Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Configuration bindings
// =========================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection(nameof(RedisSettings)));

builder.Services.Configure<HangfireSettings>(
    builder.Configuration.GetSection(nameof(HangfireSettings)));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings)));

// =========================
// FluentMigrator
// =========================

builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(runner =>
        runner
            .AddSqlServer()
            .WithGlobalConnectionString(
                builder.Configuration["DatabaseSettings:ConnectionString"])
            .ScanIn(typeof(Initial_Create_Brands).Assembly)
            .For.Migrations())
    .AddLogging(logging =>
        logging.AddFluentMigratorConsole());

// =========================
// Infrastructure
// =========================

builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddHostedService<MigrationRunnerService>();

// =========================
// API
// =========================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// =========================
// Rate limiting
// =========================

builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(
    builder.Configuration.GetSection("RateLimiting"));

builder.Services.AddInMemoryRateLimiting();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// =========================
// Middleware pipeline
// =========================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

app.Run();