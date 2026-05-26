using System.Data;

namespace Kron.Counting.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}