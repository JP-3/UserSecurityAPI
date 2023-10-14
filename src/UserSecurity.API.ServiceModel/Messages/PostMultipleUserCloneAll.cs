using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MultipleUserCloneAll/{UserNameA}/{UserNameB}", "POST")]
    public class PostMultipleUserCloneAll : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
