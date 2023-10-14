using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CloneScreenSecurity/{UserNameA}/{UserNameB}", "POST")]
    public class PostCloneScreenSecurity : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
