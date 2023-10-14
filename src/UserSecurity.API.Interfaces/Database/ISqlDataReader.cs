using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.Interfaces.Database
{
    public interface ISqlDataReader : IDisposable
    {
        bool Read();
        object this[string name] { get; }
    }
}
