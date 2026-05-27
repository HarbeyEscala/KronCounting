using Dapper;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Infrastructure.Repositories;

public sealed class BrandRepository : IBrandRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BrandRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Brand>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                Code,
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Brands
            WHERE IsDeleted = 0
            ORDER BY Name;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<Brand>(sql);
    }

    public async Task<Brand?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                Code,
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Brands
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Brand>(
            sql,
            new { Id = id });
    }

    public async Task<Brand?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                Code,
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Brands
            WHERE Code = @Code
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Brand>(
            sql,
            new { Code = code });
    }

    public async Task<Guid> CreateAsync(
        Brand brand,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.Brands
            (
                Id,
                Code,
                Name,
                Description,
                IsActive,
                IsDeleted,
                CreatedAtUtc
            )
            VALUES
            (
                @Id,
                @Code,
                @Name,
                @Description,
                @IsActive,
                @IsDeleted,
                @CreatedAtUtc
            );
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, brand);

        return brand.Id;
    }

    public async Task UpdateAsync(
        Brand brand,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Brands
            SET
                Name = @Name,
                Description = @Description,
                IsActive = @IsActive,
                UpdatedAtUtc = @UpdatedAtUtc
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, brand);
    }

    public async Task SoftDeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Brands
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