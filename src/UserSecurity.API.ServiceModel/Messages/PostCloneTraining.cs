using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CloneTraining/{UserNameA}/{UserNameB}", "POST")]
    public class PostCloneTraining : IReturn<PostResponse>
    {
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }

    }
}
