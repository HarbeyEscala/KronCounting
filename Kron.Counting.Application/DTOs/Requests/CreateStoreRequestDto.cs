namespace Kron.Counting.Application.DTOs.Requests;

public sealed class CreateStoreRequestDto
{
    public Guid TenantId { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? StoreType { get; set; }

    public string? Region { get; set; }
}