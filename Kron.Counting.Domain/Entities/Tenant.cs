namespace Kron.Counting.Domain.Entities;

public sealed class Tenant
{
    public Guid Id { get; set; }

    public Guid BrandId { get; set; }

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? DeletedAtUtc { get; set; }

    public Brand? Brand { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();

    public ICollection<Store> Stores { get; set; } = new List<Store>();
}