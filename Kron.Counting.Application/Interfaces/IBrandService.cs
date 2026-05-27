using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.DTOs.Responses;

namespace Kron.Counting.Application.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<BrandDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<BrandDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(
        CreateBrandRequestDto request,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateBrandRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}