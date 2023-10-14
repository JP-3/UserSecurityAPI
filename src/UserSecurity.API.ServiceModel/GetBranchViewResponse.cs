using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetBranchViewResponse : IHasResponseStatus
    {
        public List<BranchView> ViewableBranches { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
