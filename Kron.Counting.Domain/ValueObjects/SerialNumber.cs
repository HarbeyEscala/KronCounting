namespace Kron.Counting.Domain.ValueObjects;

public sealed record SerialNumber
{
    public string Value { get; }

    public SerialNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Serial number cannot be empty.", nameof(value));

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 100)
            throw new ArgumentException("Serial number exceeds max length.", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;

    public static implicit operator string(SerialNumber serial) => serial.Value;

    public static explicit operator SerialNumber(string value) => new(value);
}