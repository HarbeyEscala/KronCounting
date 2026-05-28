using Kron.Counting.Domain.Entities;

namespace Kron.Counting.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}