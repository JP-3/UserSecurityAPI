using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.ServiceModel.Types
{
    public class BranchView 
    {
        public string Branch { get; set; }

        public bool Active { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public string EditMoney { get; set; }
    }
}
