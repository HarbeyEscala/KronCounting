namespace Kron.Counting.Domain.ValueObjects;

public sealed record BrandCode
{
    public string Value { get; }

    public BrandCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Brand code cannot be empty.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 50)
            throw new ArgumentException("Brand code exceeds max length.", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;

    public static implicit operator string(BrandCode code) => code.Value;

    public static explicit operator BrandCode(string value) => new(value);
}