using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Kron.Counting.Application.Interfaces;

namespace Kron.Counting.Infrastructure.DeviceGateways;

public sealed class Chp015Gateway : IChp015Gateway
{
    private readonly HttpClient _httpClient;

    public Chp015Gateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(
        string deviceIp,
        string username,
        string password)
    {
        var url = $"http://{deviceIp}/api/login";

        var content =
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["username"] = username,
                    ["password"] = password
                });

        var response =
            await _httpClient.PostAsync(url, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<string> ReadLocalConfigurationAsync(
        string deviceIp)
    {
        var url = $"http://{deviceIp}/read/local";

        return await _httpClient.GetStringAsync(url);
    }

    public async Task<string> ReadNetworkConfigurationAsync(
        string deviceIp)
    {
        var url = $"http://{deviceIp}/read/netset";

        return await _httpClient.GetStringAsync(url);
    }

    public async Task ConfigureServerAsync(
        string deviceIp,
        string serverUrl)
    {
        var url = $"http://{deviceIp}/save/netset";

        var content =
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["sv"] = serverUrl
                });

        var response =
            await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }

    public async Task ConfigureWifiAsync(
        string deviceIp,
        string ssid,
        string password)
    {
        var url = $"http://{deviceIp}/save/netset";

        var content =
            new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["ssid"] = ssid,
                    ["pass"] = password
                });

        var response =
            await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }
}