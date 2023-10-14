using System;
using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/UpdateViewableBranch/{UserName}", "POST")]
    public class PostUpdateViewableBranch   :IReturn<PostResponse>
    {
        public string UserName { get; set; }

        public string BranchCode { get; set; }

        public bool Active { get; set; }

        public string UpdatedBy { get; set; }

        public bool EditMoney { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
