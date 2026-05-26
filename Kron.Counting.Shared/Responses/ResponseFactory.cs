namespace Kron.Counting.Shared.Responses;

using System;
using System.Collections.Generic;
using System.Text;
public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ErrorResponse Failure(string message, params string[] errors)
    {
        return new ErrorResponse
        {
            Message = message,
            Errors = errors
        };
    }
}