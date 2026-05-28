namespace Kron.Counting.Application.DTOs.Requests;

public sealed class ConfigureDeviceWifiRequestDto
{
    public string DeviceIp { get; set; } = default!;

    public string Ssid { get; set; } = default!;

    public string Password { get; set; } = default!;
}