using Kron.Counting.Application.DTOs.Responses;

namespace Kron.Counting.Application.Interfaces;

public interface ITelemetryService
{
    Task<long> IngestReadingAsync(
        DeviceReadingDto dto,
        CancellationToken cancellationToken = default);
}