using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MultipleUserCloneScreenSecurity/{UserNameA}/{UserNameB}", "POST")]
    public class PostMultipleUserCloneScreenSecurity : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
