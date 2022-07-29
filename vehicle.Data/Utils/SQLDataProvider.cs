
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Utils;

public class SQLDataProvider
{
    private readonly IConfiguration _configuration;

    public SQLDataProvider(IConfiguration configuration) { _configuration = configuration; }

    private string _connectionString => _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
    private static SqlParameter[] ConvertToSqlParameters(IDictionary<string, object> parameters)
    {
        List<SqlParameter> sqlParameters = new List<SqlParameter>();
        foreach (var kvPair in parameters)
        {
            sqlParameters.Add(new SqlParameter(kvPair.Key, kvPair.Value));
        }
        return sqlParameters.ToArray();
    }

    private static CommandType GetCommandType(int commandTypeNumber)
    {
        return commandTypeNumber switch
        {
            1 => CommandType.Text,
            _ => CommandType.StoredProcedure,
        };
    }

    public DataSet ExecuteStoredProcedure(string command, CommandType commandType, SqlParameter[] parameters)
    {
        DataSet result = new DataSet();
        if (new SqlConnection(_connectionString) is SqlConnection connection)
        {
            using (connection)
            {
                using SqlCommand sqlCommand = new SqlCommand(command, connection)
                {
                    CommandType = commandType,
                    CommandTimeout = 300,
                };
                sqlCommand.Parameters.AddRange(parameters);
                using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(result);
            }
        }
        else
        {
            throw new InvalidOperationException("Connection is not to a SQL Database");
        }
        return result;
    }

}