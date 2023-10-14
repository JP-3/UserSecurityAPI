using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MultipleUserCloneViewableBranches/{UserNameA}/{UserNameB}", "POST")]
    public class PostMultipleUserCloneViewableBranches : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
