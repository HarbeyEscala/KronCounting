namespace Kron.Counting.Application.DTOs.Responses;

public sealed class LoginResponseDto
{
    public string AccessToken { get; set; } = default!;

    public int ExpiresIn { get; set; }

    public string TokenType { get; set; } = "Bearer";
}