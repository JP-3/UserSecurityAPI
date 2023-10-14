using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.Interfaces.Database
{
    public interface ISqlConnection : IDisposable
    {
        ISqlConnection GetNewInstance(string connectionString);
        void Open();
        System.Data.SqlClient.SqlConnection GetConnection();
    }
}
