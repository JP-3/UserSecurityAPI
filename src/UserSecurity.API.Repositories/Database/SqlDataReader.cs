using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSecurity.API.Interfaces.Database;

namespace UserSecurity.API.Repositories.Database
{
    [ExcludeFromCodeCoverage]
    public class SqlDataReader : ISqlDataReader
    {
        private System.Data.SqlClient.SqlDataReader dataReader;

        public SqlDataReader(System.Data.SqlClient.SqlDataReader reader)
        {
            dataReader = reader;
        }

        public object this[string name]
        {
            get { return dataReader[name]; }
        }

        public void Dispose()
        {
            dataReader.Dispose();
        }

        public bool Read()
        {
            return dataReader.Read();
        }
    }
}
