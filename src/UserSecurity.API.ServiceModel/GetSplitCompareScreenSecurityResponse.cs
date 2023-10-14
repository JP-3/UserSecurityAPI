using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetCompareSplitScreenSecurityResponse : IHasResponseStatus
    {
        public SplitCompareSecurity SplitCompareSecurity { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
