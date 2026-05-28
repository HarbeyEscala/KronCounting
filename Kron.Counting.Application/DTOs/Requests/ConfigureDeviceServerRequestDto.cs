namespace Kron.Counting.Application.DTOs.Requests;

public sealed class ConfigureDeviceServerRequestDto
{
    public string DeviceIp { get; set; } = default!;

    public string ServerUrl { get; set; } = default!;
}