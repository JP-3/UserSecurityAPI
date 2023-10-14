using System;
using System.Data;
using System.Data.SqlClient;

namespace UserSecurity.API.Interfaces.Database
{
    public interface ISqlCommand : IDisposable
    {
        ISqlCommand GetNewInstance(string command, ISqlConnection connection);
        CommandType CommandType { get; set; }
        SqlParameterCollection Parameters { get; }
        ISqlDataReader ExecuteReader();
        int ExecuteNonQuery();
        object ExecuteScalar();
    }
}
