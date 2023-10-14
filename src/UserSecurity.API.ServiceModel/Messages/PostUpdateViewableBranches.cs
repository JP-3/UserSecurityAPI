using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/UpdateViewableBranches/{UserName}", "POST")]
    public class PostUpdateViewableBranches:IReturn<PostResponse>
    {
        public string UserName { get; set; }

        public string Branches { get; set; }

        public bool EditMoney { get; set; }
    }
}
