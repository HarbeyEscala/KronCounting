namespace Kron.Counting.Application.DTOs.Responses;

public sealed class UserDto
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Role { get; set; } = default!;

    public bool IsActive { get; set; }

    public DateTime? LastLoginUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}