using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/ScreenSecurity/{UserNameA}/{UserNameB}", "GET")]
    public class GetCompareScreenSecurity : IReturn<GetCompareScreenSecurityResponse>
    {       
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }
    }
}
