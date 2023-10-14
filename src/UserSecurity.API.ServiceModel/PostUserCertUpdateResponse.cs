using ServiceStack;
using UserSecurity.API.ServiceModel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.ServiceModel
{
    public class PostUserCertUpdateResponse: IHasResponseStatus
    {
        public List<UserCertification> UserCertificationList { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

    }
}

