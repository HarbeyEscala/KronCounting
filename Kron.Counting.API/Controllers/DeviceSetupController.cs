using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kron.Counting.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/device-setup")]
public sealed class DeviceSetupController : ControllerBase
{
    private readonly IDeviceProvisioningService _provisioningService;

    public DeviceSetupController(
        IDeviceProvisioningService provisioningService)
    {
        _provisioningService = provisioningService;
    }

    [HttpPost("configure-wifi")]
    public async Task<IActionResult> ConfigureWifi(
        [FromBody] ConfigureDeviceWifiRequestDto request)
    {
        await _provisioningService.ConfigureWifiAsync(request);

        return Ok(new
        {
            message = "Device WiFi configured successfully."
        });
    }

    [HttpPost("configure-server")]
    public async Task<IActionResult> ConfigureServer(
        [FromBody] ConfigureDeviceServerRequestDto request)
    {
        await _provisioningService.ConfigureServerAsync(request);

        return Ok(new
        {
            message = "Device server configured successfully."
        });
    }

    [HttpGet("{deviceIp}/local-config")]
    public async Task<IActionResult> ReadLocalConfiguration(
        string deviceIp)
    {
        var result =
            await _provisioningService
                .ReadLocalConfigurationAsync(deviceIp);

        return Ok(result);
    }

    [HttpGet("{deviceIp}/network-config")]
    public async Task<IActionResult> ReadNetworkConfiguration(
        string deviceIp)
    {
        var result =
            await _provisioningService
                .ReadNetworkConfigurationAsync(deviceIp);

        return Ok(result);
    }
}