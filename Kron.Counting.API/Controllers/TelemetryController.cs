using Kron.Counting.Application.Adapters;
using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kron.Counting.API.Controllers;

[ApiController]
[Route("api/v1/internal/device-readings")]
public sealed class TelemetryController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;
    private readonly IDeviceRepository _deviceRepository;

    public TelemetryController(
        ITelemetryService telemetryService,
        IDeviceRepository deviceRepository)
    {
        _telemetryService = telemetryService;
        _deviceRepository = deviceRepository;
    }

    [HttpPost]
    public async Task<IActionResult> IngestReading(
        [FromBody] RawChp015TelemetryRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!Request.Headers.TryGetValue(
                "X-Device-Key",
                out var apiKey))
        {
            return Unauthorized(new
            {
                message = "Missing device API key."
            });
        }

        var device =
            await _deviceRepository.GetByApiKeyAsync(
                apiKey!,
                cancellationToken);

        if (device is null)
        {
            return Unauthorized(new
            {
                message = "Invalid device API key."
            });
        }

        if (!device.IsActive || device.IsDeleted)
        {
            return Unauthorized(new
            {
                message = "Device inactive."
            });
        }

        var normalized =
            Chp015TelemetryAdapter.Normalize(request);

        var id =
            await _telemetryService.IngestReadingAsync(
                device.Id,
                normalized,
                cancellationToken);

        return Created(string.Empty, new { id });
    }
}