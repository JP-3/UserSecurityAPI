using ServiceStack;
using System.Collections.Generic;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CrossEnvironmentClone/{CloneToUsername}", "POST")]
    public class PostCrossEnvironmentClone : IReturn<PostResponse>
    {
        public string CloneToUsername { get; set; }

        public List<ScreenSecurity> ScreenSecurities { get; set; }
    }
}