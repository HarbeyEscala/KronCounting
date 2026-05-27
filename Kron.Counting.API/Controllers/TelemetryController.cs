using Kron.Counting.Application.DTOs.Responses;
using Kron.Counting.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kron.Counting.API.Controllers;

[ApiController]
[Route("api/v1/internal/device-readings")]
public sealed class TelemetryController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;

    public TelemetryController(ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    [HttpPost]
    public async Task<IActionResult> IngestReading(
        [FromBody] DeviceReadingDto request,
        CancellationToken cancellationToken)
    {
        var id = await _telemetryService.IngestReadingAsync(request, cancellationToken);

        return Created(string.Empty, new { id });
    }
}