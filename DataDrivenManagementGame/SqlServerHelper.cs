using System;
using System.Data;
using Microsoft.Data.SqlClient;

public class SqlServerHelper
{
    private SqlConnection _connection;

    // Connection string should include server name, database name, and authentication details.
    public string ConnectionString { get; }

    public SqlServerHelper(string connectionString)
    {
        ConnectionString = connectionString;
    }

    // Method to create connection
    public void CreateConnection()
    {
        if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            return;

        _connection = new SqlConnection(ConnectionString);
        _connection.Open();
    }

    // Method to execute a query and return the result as a DataTable
    public DataTable ExecuteQuery(string query)
    {
        using var command = new SqlCommand(query, _connection);
        using var dataAdapter = new SqlDataAdapter(command);

        var resultTable = new DataTable();
        dataAdapter.Fill(resultTable);

        return resultTable;
    }



    // Method to dispose connection
    public void DisposeConnection()
    {
        if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            _connection.Close();
        _connection?.Dispose();
    }
}