namespace Kron.Counting.Application.DTOs.Requests;

public sealed class UpdateTenantRequestDto
{
    public string? Name { get; set; }

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }

    public string? Locale { get; set; }

    public bool IsActive { get; set; }
}