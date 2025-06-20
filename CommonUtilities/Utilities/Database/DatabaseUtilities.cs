// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Serilog;

namespace CommonUtilities.Utilities.Database;

/// <summary>
///     Provides utility methods for database operations.
/// </summary>
public static class DatabaseUtilities
{
    /// <summary>
    ///     Executes a stored procedure asynchronously and maps the results to a list of objects.
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

            if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
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
    ///     Maps data from a DbDataReader to a list of objects of type T.
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
                var columnMapDetails = colMapping[prop.Name.ToLower()];
                if (columnMapDetails.ColumnOrdinal.HasValue)
                {
                    object val = dr.GetValue(columnMapDetails.ColumnOrdinal.Value);
                    prop.SetValue(obj, val == DBNull.Value ? null : val);
                }
            }

            objList.Add(obj);
        }

        return objList;
    }
}