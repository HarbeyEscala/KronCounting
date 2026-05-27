using System.Text.RegularExpressions;

namespace Kron.Counting.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex =
        new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalized))
            throw new ArgumentException("Invalid email format.", nameof(value));

        Value = normalized;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    public static explicit operator Email(string value) => new(value);
}