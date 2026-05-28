using Kron.Counting.Application.DTOs.Requests;
using Kron.Counting.Application.DTOs.Responses;

namespace Kron.Counting.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}