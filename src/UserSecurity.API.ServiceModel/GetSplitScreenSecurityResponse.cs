using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetSplitScreenSecurityResponse : IHasResponseStatus
    {
        public SplitScreenSecurity SplitScreenSecurity { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
