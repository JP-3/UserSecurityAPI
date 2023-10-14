using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CloneAll/{UserNameA}/{UserNameB}", "POST")]
    public class PostCloneAll : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
