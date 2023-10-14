using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/DeleteViewableBranches/{UserName}", "POST")]
    public class PostDeleteViewableBranches : IReturn<PostResponse>
    {
        public string UserName { get; set; }
    }
}
