namespace Kron.Counting.Shared.Settings;

public sealed class JwtSettings
{
    public string SecretKey { get; set; } = default!;

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;

    public int AccessTokenExpirationMinutes { get; set; }
}