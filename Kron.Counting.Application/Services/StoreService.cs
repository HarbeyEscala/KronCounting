using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.DTOs.Responses;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Application.Mappings;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Services;

public sealed class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly ITenantRepository _tenantRepository;

    public StoreService(
        IStoreRepository storeRepository,
        ITenantRepository tenantRepository)
    {
        _storeRepository = storeRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task<IEnumerable<StoreDto>> GetByTenantIdAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        var stores =
            await _storeRepository.GetByTenantIdAsync(
                tenantId,
                cancellationToken);

        return stores.Select(x => x.ToDto());
    }

    public async Task<StoreDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var store =
            await _storeRepository.GetByIdAsync(
                id,
                cancellationToken);

        return store?.ToDto();
    }

    public async Task<Guid> CreateAsync(
        CreateStoreRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var tenant =
            await _tenantRepository.GetByIdAsync(
                request.TenantId,
                cancellationToken);

        if (tenant is null)
            throw new KeyNotFoundException("Tenant not found.");

        var existing =
            await _storeRepository.GetByCodeAsync(
                request.TenantId,
                request.Code,
                cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                $"Store with code '{request.Code}' already exists.");

        var entity = new Store
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            Code = request.Code.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            StoreType = request.StoreType?.Trim(),
            Region = request.Region?.Trim(),
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        return await _storeRepository.CreateAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateStoreRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var existing =
            await _storeRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException("Store not found.");

        existing.Name = request.Name.Trim();
        existing.Description = request.Description?.Trim();
        existing.StoreType = request.StoreType?.Trim();
        existing.Region = request.Region?.Trim();
        existing.IsActive = request.IsActive;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        await _storeRepository.UpdateAsync(existing, cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var existing =
            await _storeRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException("Store not found.");

        await _storeRepository.SoftDeleteAsync(id, cancellationToken);
    }
}