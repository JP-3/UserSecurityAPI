using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/ScreenSecurity/{UserName}", "GET")]
    public class GetScreenSecurity : IReturn<GetScreenSecurityResponse>
    {       
        public string UserName { get; set; }
    }
}
