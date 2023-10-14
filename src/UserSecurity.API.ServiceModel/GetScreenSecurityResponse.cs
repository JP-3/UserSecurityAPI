using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetScreenSecurityResponse : IHasResponseStatus
    {
        public List<ScreenSecurity> ScreenSecurities { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
