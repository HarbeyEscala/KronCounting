using Kron.Counting.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kron.Counting.API.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stores/{storeId:guid}/snapshot")]
    public async Task<IActionResult> GetSnapshot(
        Guid storeId,
        CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetSnapshotAsync(storeId, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("stores/{storeId:guid}/hourly")]
    public async Task<IActionResult> GetHourlyMetrics(
        Guid storeId,
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetHourlyMetricsAsync(
            storeId,
            fromDate,
            toDate,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("stores/{storeId:guid}/daily")]
    public async Task<IActionResult> GetDailyMetrics(
        Guid storeId,
        [FromQuery] DateOnly fromDate,
        [FromQuery] DateOnly toDate,
        CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetDailyMetricsAsync(
            storeId,
            fromDate,
            toDate,
            cancellationToken);

        return Ok(result);
    }
}