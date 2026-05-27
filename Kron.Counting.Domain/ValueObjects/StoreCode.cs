namespace Kron.Counting.Domain.ValueObjects;

public sealed record StoreCode
{
    public string Value { get; }

    public StoreCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Store code cannot be empty.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 50)
            throw new ArgumentException("Store code exceeds max length.", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;

    public static implicit operator string(StoreCode code) => code.Value;

    public static explicit operator StoreCode(string value) => new(value);
}