using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Interfaces;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetByBrandIdAsync(Guid brandId, CancellationToken cancellationToken = default);

    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Tenant?> GetByCodeAsync(Guid brandId, string code, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(Tenant tenant, CancellationToken cancellationToken = default);

    Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}