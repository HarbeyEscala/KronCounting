namespace Kron.Counting.Shared.Exceptions;

public sealed class ValidationException : AppException
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}