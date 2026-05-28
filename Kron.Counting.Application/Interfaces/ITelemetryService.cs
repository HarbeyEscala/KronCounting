using Kron.Counting.Application.DTOs.Requests;

namespace Kron.Counting.Application.Interfaces;

public interface ITelemetryService
{
    Task<long> IngestReadingAsync(
        Guid deviceId,
        IngestCounterSnapshotRequestDto request,
        CancellationToken cancellationToken = default);
}