using Kron.Counting.Application.DTOs.Responses;
using Kron.Counting.Application.Interfaces;
using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Services;

public sealed class TelemetryService : ITelemetryService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceReadingRepository _deviceReadingRepository;
    private readonly IDashboardRepository _dashboardRepository;

    public TelemetryService(
        IDeviceRepository deviceRepository,
        IDeviceReadingRepository deviceReadingRepository,
        IDashboardRepository dashboardRepository)
    {
        _deviceRepository = deviceRepository;
        _deviceReadingRepository = deviceReadingRepository;
        _dashboardRepository = dashboardRepository;
    }

    public async Task<long> IngestReadingAsync(
        DeviceReadingDto dto,
        CancellationToken cancellationToken = default)
    {
        var device =
            await _deviceRepository.GetByIdAsync(
                dto.DeviceId,
                cancellationToken);

        if (device is null)
            throw new KeyNotFoundException("Device not found.");

        if (!device.IsActive || device.IsDeleted)
            throw new InvalidOperationException("Device is inactive.");

        var reading = new DeviceReading
        {
            DeviceId = dto.DeviceId,
            ReadingTimestampUtc = dto.ReadingTimestampUtc,
            PeopleIn = dto.PeopleIn,
            PeopleOut = dto.PeopleOut,
            Occupancy = dto.Occupancy,
            ConfidenceScore = dto.ConfidenceScore,
            RawPayloadJson = dto.RawPayloadJson,
            CreatedAtUtc = DateTime.UtcNow
        };

        var readingId =
            await _deviceReadingRepository.CreateAsync(
                reading,
                cancellationToken);

        await _deviceRepository.UpdateHeartbeatAsync(
            dto.DeviceId,
            DateTime.UtcNow,
            true,
            cancellationToken);

        var snapshot =
            await _dashboardRepository.GetSnapshotByStoreIdAsync(
                device.StoreId,
                cancellationToken);

        if (snapshot is null)
        {
            snapshot = new LiveDashboardSnapshot
            {
                Id = Guid.NewGuid(),
                StoreId = device.StoreId,
                CurrentOccupancy = dto.Occupancy,
                TodayIn = dto.PeopleIn,
                TodayOut = dto.PeopleOut,
                LastReadingAtUtc = dto.ReadingTimestampUtc,
                UpdatedAtUtc = DateTime.UtcNow
            };
        }
        else
        {
            snapshot.CurrentOccupancy = dto.Occupancy;
            snapshot.TodayIn += dto.PeopleIn;
            snapshot.TodayOut += dto.PeopleOut;
            snapshot.LastReadingAtUtc = dto.ReadingTimestampUtc;
            snapshot.UpdatedAtUtc = DateTime.UtcNow;
        }

        await _dashboardRepository.UpsertSnapshotAsync(
            snapshot,
            cancellationToken);

        return readingId;
    }
}