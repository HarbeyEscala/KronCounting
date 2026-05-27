namespace Kron.Counting.Application.DTOs.Requests;

public sealed class UpdateUserRequestDto
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Role { get; set; } = default!;

    public bool IsActive { get; set; }
}