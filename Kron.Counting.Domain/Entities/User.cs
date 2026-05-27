namespace Kron.Counting.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public string Role { get; set; } = default!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? LastLoginUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? DeletedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
}