using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/ScreenSecurity/{UserName}/BranchView", "GET")]
    public class GetEmployeeBranchView
    {       
        public string UserName { get; set; }
    }
}
