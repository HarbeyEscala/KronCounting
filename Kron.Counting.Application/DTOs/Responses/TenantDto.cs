namespace Kron.Counting.Application.DTOs.Responses;

public sealed class TenantDto
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}