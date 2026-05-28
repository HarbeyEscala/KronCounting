namespace Kron.Counting.Application.DTOs.Requests;

public sealed class CreateTenantRequestDto
{
    public Guid BrandId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }

    public string? Locale { get; set; }
}