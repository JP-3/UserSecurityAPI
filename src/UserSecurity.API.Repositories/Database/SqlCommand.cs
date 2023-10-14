using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using UserSecurity.API.Interfaces.Database;

namespace UserSecurity.API.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class SqlCommand : ISqlCommand
    {
        private System.Data.SqlClient.SqlCommand sqlCommand;


        public SqlCommand() { }

        private SqlCommand(System.Data.SqlClient.SqlCommand command)
        {
            sqlCommand = command;
        }

        public CommandType CommandType
        {
            get
            {
                SanityCheck();
                return sqlCommand.CommandType;
            }
            set
            {
                SanityCheck();
                sqlCommand.CommandType = value;
            }
        }

        public SqlParameterCollection Parameters
        {
            get
            {
                SanityCheck();
                return sqlCommand.Parameters;
            }
        }

        public void Dispose()
        {
            SanityCheck();
            sqlCommand.Dispose();
        }

        public ISqlDataReader ExecuteReader()
        {
            SanityCheck();
            return new SqlDataReader(sqlCommand.ExecuteReader());
        }

        public int ExecuteNonQuery()
        {
            SanityCheck();
            return sqlCommand.ExecuteNonQuery();
        }

        public object ExecuteScalar()
        {
            SanityCheck();
            return sqlCommand.ExecuteScalar();
        }

        public ISqlCommand GetNewInstance(string command, ISqlConnection connection)
        {
            return new SqlCommand(new System.Data.SqlClient.SqlCommand(command, connection.GetConnection()));
        }

        private void SanityCheck()
        {
            if (sqlCommand == null)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
