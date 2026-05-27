namespace Kron.Counting.Application.DTOs.Requests;

public sealed class CreateTenantRequestDto
{
    public Guid BrandId { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}