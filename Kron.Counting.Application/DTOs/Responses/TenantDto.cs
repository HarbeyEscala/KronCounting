namespace Kron.Counting.Application.DTOs.Responses;

public sealed class TenantDto
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string TimeZone { get; set; } = default!;

    public string Currency { get; set; } = default!;

    public string Locale { get; set; } = default!;

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}