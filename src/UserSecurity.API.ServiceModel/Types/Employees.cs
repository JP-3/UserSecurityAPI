using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.ServiceModel.Types
{
    public class Employees
    {
        public string InvalidEmployees { get; set; }

        public List<string> ValidEmployees { get; set; }
    }
}
