using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.Interfaces;

namespace Kron.Counting.Application.Services;

public sealed class DeviceProvisioningService : IDeviceProvisioningService
{
    private readonly IChp015Gateway _gateway;

    public DeviceProvisioningService(
        IChp015Gateway gateway)
    {
        _gateway = gateway;
    }

    public async Task ConfigureWifiAsync(
        ConfigureDeviceWifiRequestDto request)
    {
        await _gateway.ConfigureWifiAsync(
            request.DeviceIp,
            request.Ssid,
            request.Password);
    }

    public async Task ConfigureServerAsync(
        ConfigureDeviceServerRequestDto request)
    {
        await _gateway.ConfigureServerAsync(
            request.DeviceIp,
            request.ServerUrl);
    }

    public async Task<string> ReadLocalConfigurationAsync(
        string deviceIp)
    {
        return await _gateway.ReadLocalConfigurationAsync(deviceIp);
    }

    public async Task<string> ReadNetworkConfigurationAsync(
        string deviceIp)
    {
        return await _gateway.ReadNetworkConfigurationAsync(deviceIp);
    }
}