using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CloneViewableBranches/{UserNameA}/{UserNameB}", "POST")]
    public class PostCloneViewableBranches : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
