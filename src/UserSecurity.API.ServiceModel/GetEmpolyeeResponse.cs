using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class GetEmployeeResponse : IHasResponseStatus
    {
        public EmployeeInfo Employee { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
