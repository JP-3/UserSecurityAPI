using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/GrantAllUserCerts/{UserName}", "POST")]
    public class PostGrantAllUserCerts : IReturn<PostResponse>
    {
        public string UserName{ get; set; }
    }
}