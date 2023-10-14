using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/MultipleUserCloneTraining/{UserNameA}/{UserNameB}", "POST")]
    public class PostMultipleUserCloneTraining : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
