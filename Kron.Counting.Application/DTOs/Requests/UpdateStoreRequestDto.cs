namespace Kron.Counting.Application.DTOs.Requests;

public sealed class UpdateStoreRequestDto
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public string? StoreType { get; set; }

    public string? Region { get; set; }

    public bool IsActive { get; set; }
}