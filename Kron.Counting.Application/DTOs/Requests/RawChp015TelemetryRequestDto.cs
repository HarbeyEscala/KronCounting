using System.Text.Json.Serialization;

namespace Kron.Counting.Application.DTOs.Requests;

public sealed class RawChp015TelemetryRequestDto
{
    [JsonPropertyName("serialNumber")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("readingTimestampUtc")]
    public DateTime? ReadingTimestampUtc { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("totalIn")]
    public int? TotalIn { get; set; }

    [JsonPropertyName("in")]
    public int? In { get; set; }

    [JsonPropertyName("total_enter")]
    public int? TotalEnter { get; set; }

    [JsonPropertyName("totalOut")]
    public int? TotalOut { get; set; }

    [JsonPropertyName("out")]
    public int? Out { get; set; }

    [JsonPropertyName("total_exit")]
    public int? TotalExit { get; set; }
}