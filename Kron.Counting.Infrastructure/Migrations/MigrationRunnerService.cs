using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kron.Counting.Infrastructure.Migrations;

public sealed class MigrationRunnerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationRunnerService> _logger;

    public MigrationRunnerService(
        IServiceProvider serviceProvider,
        ILogger<MigrationRunnerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        //_logger.LogInformation("Running database migrations...");

        runner.MigrateUp();

        _logger.LogInformation("Database migrations completed.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}