using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using Serilog;

namespace CommonUtilities.Utilities;

/// <summary>
/// Provides utility methods for database operations.
/// </summary>
public static class DatabaseUtilities
{
    /// <summary>
    /// Executes a stored procedure asynchronously and maps the results to a list of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects to map the results to.</typeparam>
    /// <param name="connectionStrings">The database connection string.</param>
    /// <param name="spName">The name of the stored procedure to execute.</param>
    /// <param name="parameters">A list of SqlParameter objects to pass to the stored procedure.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of a List of objects of type T.</returns>
    public static async Task<List<T>> ExecuteAsync<T>(string connectionStrings, string spName,
        List<SqlParameter> parameters)
    {
        List<T> results = new();

        await using SqlConnection connection = new SqlConnection(connectionStrings);
        try
        {
            await using DbCommand command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                foreach (SqlParameter param in parameters)
                    command.Parameters.Add(param);

            if (command.Connection.State == ConnectionState.Closed)
                await command.Connection.OpenAsync();

            await using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                results = await MapToListAsync<T>(reader);
            }

            connection.Close();
        }
        catch (Exception ex)
        {
            Log.Error("ExecuteAsync - Exception, Error Message: {0}", ex.Message);
        }
        finally
        {
            connection.Close();
        }

        return results;
    }

    /// <summary>
    /// Maps data from a DbDataReader to a list of objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to map the data to.</typeparam>
    /// <param name="dr">The DbDataReader containing the data to map.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of a List of objects of type T.</returns>
    private static async Task<List<T>> MapToListAsync<T>(DbDataReader dr)
    {
        List<T> objList = new();
        IEnumerable<PropertyInfo> props = typeof(T).GetRuntimeProperties();

        Dictionary<string, DbColumn> colMapping = dr.GetColumnSchema()
            .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
            .ToDictionary(key => key.ColumnName.ToLower());

        if (!dr.HasRows) return objList;
        while (await dr.ReadAsync())
        {
            T obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in props)
            {
                object val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                prop.SetValue(obj, val == DBNull.Value ? null : val);
            }

            objList.Add(obj);
        }

        return objList;
    }
}