using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/ScreenSecurity/{UserName}/BranchView", "GET")]
    public class GetBranchView : IReturn<GetBranchViewResponse>
    {       
        public string UserName { get; set; }
    }
}
