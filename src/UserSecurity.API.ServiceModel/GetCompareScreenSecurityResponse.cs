using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetCompareScreenSecurityResponse : IHasResponseStatus
    {
        public List<CompareSecurity> ComparedSecurities { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
