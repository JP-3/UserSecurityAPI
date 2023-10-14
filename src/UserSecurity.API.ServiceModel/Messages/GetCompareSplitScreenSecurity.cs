using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/CompareSplitScreenSecurity/{UserNameA}/{UserNameB}", "GET")]
    public class GetCompareSplitScreenSecurity : IReturn<GetCompareSplitScreenSecurityResponse>
    {       
        public string UserNameA { get; set; }

        public string UserNameB { get; set; }
    }
}
