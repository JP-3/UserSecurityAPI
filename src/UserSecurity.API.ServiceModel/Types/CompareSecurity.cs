using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.ServiceModel.Types
{
    public class CompareSecurity : ScreenSecurity
    {
        public string AccessLevel2 { get; set; }

        public List<SecurityLevel> SecurityLevels2 { get; set; }

    }
}
