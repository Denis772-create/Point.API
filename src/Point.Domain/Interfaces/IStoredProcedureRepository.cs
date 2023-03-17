using Microsoft.Data.SqlClient;

namespace Point.Domain.Interfaces;

public interface IStoredProcedureRepository
{
    Task<IEnumerable<T>> ExecuteStoredProcedure<T>(string storedProcedureName, params SqlParameter[] parameters) where T : class;
    Task<int> ExecuteStoredProcedureNonQuery(string storedProcedureName, params SqlParameter[] parameters);
}