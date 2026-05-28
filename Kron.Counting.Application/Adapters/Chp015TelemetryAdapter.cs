using Kron.Counting.Application.DTOs.Requests;

namespace Kron.Counting.Application.Adapters;

public static class Chp015TelemetryAdapter
{
    public static IngestCounterSnapshotRequestDto Normalize(
        RawChp015TelemetryRequestDto raw)
    {
        var serialNumber =
            raw.SerialNumber
            ?? raw.DeviceId
            ?? throw new InvalidOperationException(
                "Device serial number not found.");

        var timestamp =
            raw.ReadingTimestampUtc
            ?? raw.Timestamp
            ?? DateTime.UtcNow;

        var totalIn =
            raw.TotalIn
            ?? raw.In
            ?? raw.TotalEnter
            ?? 0;

        var totalOut =
            raw.TotalOut
            ?? raw.Out
            ?? raw.TotalExit
            ?? 0;

        return new IngestCounterSnapshotRequestDto
        {
            SerialNumber = serialNumber,
            ReadingTimestampUtc = timestamp,
            TotalIn = totalIn,
            TotalOut = totalOut
        };
    }
}