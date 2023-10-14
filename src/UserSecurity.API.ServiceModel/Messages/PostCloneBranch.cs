using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CloneBranch/{UserNameA}/{UserNameB}", "POST")]
    public class PostCloneBranch : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
