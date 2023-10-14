using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/UpdateScreenSecurity/{UserName}", "POST")]
    public class PostUpdateScreenSecurity : IReturn<PostResponse>
    {
        public string UserName { get; set; }

        public int ScreenID { get; set; }

        public int AccessLevel { get; set; }
    }
}
