using System;
using System;
using System.Diagnostics.CodeAnalysis;
using UserSecurity.API.Interfaces.Database;

namespace UserSecurity.API.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class SqlConnection : ISqlConnection
    {
        private System.Data.SqlClient.SqlConnection sqlConnection;

        public SqlConnection() { }

        private SqlConnection(System.Data.SqlClient.SqlConnection connection)
        {
            sqlConnection = connection;
        }

        public void Dispose()
        {
            SanityCheck();
            sqlConnection.Dispose();
        }

        public System.Data.SqlClient.SqlConnection GetConnection()
        {
            SanityCheck();
            return sqlConnection;
        }

        public ISqlConnection GetNewInstance(string connectionString)
        {
            return new SqlConnection(new System.Data.SqlClient.SqlConnection(connectionString));
        }

        public void Open()
        {
            SanityCheck();
            sqlConnection.Open();
        }

        private void SanityCheck()
        {
            if (sqlConnection == null)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

