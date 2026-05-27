namespace Kron.Counting.Application.DTOs.Requests;

public sealed class UpdateBrandRequestDto
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}