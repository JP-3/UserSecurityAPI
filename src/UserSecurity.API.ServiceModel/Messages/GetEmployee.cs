using ServiceStack;

namespace UserSecurity.API.ServiceModel.Messages
{
    [Route("/Employee/{UserName}", "GET")]
    public class GetEmployee : IReturn<GetEmployeeResponse>
    {       
        public string UserName { get; set; }
    }
}
