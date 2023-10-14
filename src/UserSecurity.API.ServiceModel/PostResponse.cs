using System.Collections.Generic;
using ServiceStack;
using UserSecurity.API.ServiceModel.Types;

namespace UserSecurity.API.ServiceModel
{
    public class PostResponse : IHasResponseStatus
    {     
        public int RecordsUpdated { get; set; }

        public string Message { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
