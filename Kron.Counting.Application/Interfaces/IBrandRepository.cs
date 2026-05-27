using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Brand?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(Brand brand, CancellationToken cancellationToken = default);

    Task UpdateAsync(Brand brand, CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}