using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MoveBranch/{UserName}", "POST")]
    public class PostMoveBranch : IReturn<PostResponse>
    {
        public string Branch { get; set; }

        public string BranchManager { get; set; }

        public string UserName { get; set; }

        public string Department { get; set; }
    }
}
