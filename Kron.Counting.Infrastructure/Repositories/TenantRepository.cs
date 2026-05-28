using Dapper;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Infrastructure.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TenantRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Tenant>> GetByBrandIdAsync(
        Guid brandId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                BrandId,
                Code,
                Name,
                TimeZone,
                Currency,
                Locale,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Tenants
            WHERE BrandId = @BrandId
              AND IsDeleted = 0
            ORDER BY Name;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Tenant>(
            sql,
            new { BrandId = brandId });
    }

    public async Task<Tenant?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                BrandId,
                Code,
                Name,
                TimeZone,
                Currency,
                Locale,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Tenants
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Tenant>(
            sql,
            new { Id = id });
    }

    public async Task<Tenant?> GetByCodeAsync(
        Guid brandId,
        string code,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                BrandId,
                Code,
                Name,
                TimeZone,
                Currency,
                Locale,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Tenants
            WHERE BrandId = @BrandId
              AND Code = @Code
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Tenant>(
            sql,
            new
            {
                BrandId = brandId,
                Code = code
            });
    }

    public async Task<Guid> CreateAsync(
        Tenant tenant,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.Tenants
            (
                Id,
                BrandId,
                Code,
                Name,
                TimeZone,
                Currency,
                Locale,
                IsActive,
                IsDeleted,
                CreatedAtUtc
            )
            VALUES
            (
                @Id,
                @BrandId,
                @Code,
                @Name,
                @TimeZone,
                @Currency,
                @Locale,
                @IsActive,
                @IsDeleted,
                @CreatedAtUtc
            );
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, tenant);

        return tenant.Id;
    }

    public async Task UpdateAsync(
        Tenant tenant,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Tenants
            SET
                Name = @Name,
                TimeZone = @TimeZone,
                Currency = @Currency,
                Locale = @Locale,
                IsActive = @IsActive,
                UpdatedAtUtc = @UpdatedAtUtc
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, tenant);
    }

    public async Task SoftDeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Tenants
            SET
                IsDeleted = 1,
                IsActive = 0,
                DeletedAtUtc = @DeletedAtUtc
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                DeletedAtUtc = DateTime.UtcNow
            });
    }
}