namespace Kron.Counting.Application.DTOs.Requests;

public sealed class CreateBrandRequestDto
{
    public string? Code { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}