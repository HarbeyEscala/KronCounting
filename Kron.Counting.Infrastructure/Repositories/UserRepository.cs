using Dapper;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<User>> GetByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                TenantId,
                Email,
                FirstName,
                LastName,
                PasswordHash,
                Role,
                IsActive,
                IsDeleted,
                LastLoginUtc,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Users
            WHERE TenantId = @TenantId
              AND IsDeleted = 0
            ORDER BY FirstName, LastName;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<User>(
            sql,
            new { TenantId = tenantId });
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                TenantId,
                Email,
                FirstName,
                LastName,
                PasswordHash,
                Role,
                IsActive,
                IsDeleted,
                LastLoginUtc,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Users
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(
        Guid tenantId,
        string email,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                Id,
                TenantId,
                Email,
                FirstName,
                LastName,
                PasswordHash,
                Role,
                IsActive,
                IsDeleted,
                LastLoginUtc,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Users
            WHERE TenantId = @TenantId
              AND Email = @Email
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new
            {
                TenantId = tenantId,
                Email = email
            });
    }
    //PETICION GLOBAL ::)
    public async Task<User?> GetByGlobalEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
        {
            const string sql = """
            SELECT
                Id,
                TenantId,
                Email,
                FirstName,
                LastName,
                PasswordHash,
                Role,
                IsActive,
                IsDeleted,
                LastLoginUtc,
                CreatedAtUtc,
                UpdatedAtUtc,
                DeletedAtUtc
            FROM dbo.Users
            WHERE Email = @Email
              AND IsDeleted = 0;
        """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(
                sql,
                new
                {
                    Email = email.Trim().ToLower()
                });
        }

    public async Task<Guid> CreateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.Users
            (
                Id,
                TenantId,
                Email,
                FirstName,
                LastName,
                PasswordHash,
                Role,
                IsActive,
                IsDeleted,
                LastLoginUtc,
                CreatedAtUtc
            )
            VALUES
            (
                @Id,
                @TenantId,
                @Email,
                @FirstName,
                @LastName,
                @PasswordHash,
                @Role,
                @IsActive,
                @IsDeleted,
                @LastLoginUtc,
                @CreatedAtUtc
            );
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, user);

        return user.Id;
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Users
            SET
                FirstName = @FirstName,
                LastName = @LastName,
                Role = @Role,
                IsActive = @IsActive,
                UpdatedAtUtc = @UpdatedAtUtc
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, user);
    }

    public async Task UpdateLastLoginAsync(
        Guid id,
        DateTime lastLoginUtc,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Users
            SET
                LastLoginUtc = @LastLoginUtc,
                UpdatedAtUtc = @UpdatedAtUtc
            WHERE Id = @Id
              AND IsDeleted = 0;
        """;

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                LastLoginUtc = lastLoginUtc,
                UpdatedAtUtc = DateTime.UtcNow
            });
    }

    public async Task SoftDeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Users
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