using Kron.Counting.Application.DTOs.Requests;

namespace Kron.Counting.Application.Interfaces;

public interface IDeviceProvisioningService
{
    Task ConfigureWifiAsync(
        ConfigureDeviceWifiRequestDto request);

    Task ConfigureServerAsync(
        ConfigureDeviceServerRequestDto request);

    Task<string> ReadLocalConfigurationAsync(
        string deviceIp);

    Task<string> ReadNetworkConfigurationAsync(
        string deviceIp);
}