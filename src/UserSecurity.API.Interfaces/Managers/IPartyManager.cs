using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.Interfaces.Managers
{
    public interface IPartyManager
    {
        ReadBranchResponse GetBranch(string searchString);
    }
}
