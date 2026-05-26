namespace Kron.Counting.Shared.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message)
    {
    }
}