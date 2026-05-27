namespace Kron.Counting.Domain.ValueObjects;

public sealed record TenantCode
{
    public string Value { get; }

    public TenantCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tenant code cannot be empty.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 50)
            throw new ArgumentException("Tenant code exceeds max length.", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;

    public static implicit operator string(TenantCode code) => code.Value;

    public static explicit operator TenantCode(string value) => new(value);
}