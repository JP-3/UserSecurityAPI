using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MultipleUserCloneBranch/{UserNameA}/{UserNameB}", "POST")]
    public class PostMultipleUserCloneBranch : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
