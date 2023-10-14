using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/SplitScreenSecurity/{UserName}", "GET")]
    public class GetSplitScreenSecurity : IReturn<GetSplitScreenSecurityResponse>
    {       
        public string UserName { get; set; }
    }
}
