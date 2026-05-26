using Dapper;
using Microsoft.AspNetCore.Mvc;
using Kron.Counting.Application.Interfaces;

namespace Kron.Counting.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class DatabaseController : ControllerBase
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DatabaseController(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var result = await connection.QueryFirstAsync<string>(
            "SELECT 'SQL CONNECTED'");

        return Ok(new
        {
            message = result
        });
    }
}