using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.DTOs.Responses;

namespace Kron.Counting.Application.Interfaces;

public interface ITenantService
{
    Task<IEnumerable<TenantDto>> GetByBrandIdAsync(
        Guid brandId,
        CancellationToken cancellationToken = default);

    Task<TenantDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(
        CreateTenantRequestDto request,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateTenantRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}