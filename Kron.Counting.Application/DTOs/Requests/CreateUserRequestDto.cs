namespace Kron.Counting.Application.DTOs.Requests;

public sealed class CreateUserRequestDto
{
    public Guid TenantId { get; set; }

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Role { get; set; } = default!;
}