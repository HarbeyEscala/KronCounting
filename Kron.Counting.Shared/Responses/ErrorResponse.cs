namespace Kron.Counting.Shared.Responses;

using System;
using System.Collections.Generic;
using System.Text;
public sealed class ErrorResponse
{
    public bool Success => false;

    public string Message { get; init; } = string.Empty;

    public IEnumerable<string> Errors { get; init; } = [];
}