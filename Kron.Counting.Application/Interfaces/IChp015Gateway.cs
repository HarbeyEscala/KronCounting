namespace Kron.Counting.Application.Interfaces;

public interface IChp015Gateway
{
    Task<bool> LoginAsync(
        string deviceIp,
        string username,
        string password);

    Task<string> ReadLocalConfigurationAsync(
        string deviceIp);

    Task<string> ReadNetworkConfigurationAsync(
        string deviceIp);

    Task ConfigureServerAsync(
        string deviceIp,
        string serverUrl);

    Task ConfigureWifiAsync(
        string deviceIp,
        string ssid,
        string password);
}