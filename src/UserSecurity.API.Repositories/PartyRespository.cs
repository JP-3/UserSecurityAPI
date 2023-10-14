using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyV5.ServiceModel.Messages;
using UserSecurity.API.Interfaces.Repositories;
using ServiceStack;

namespace UserSecurity.API.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly IServiceClient _jsonServiceClient;

        public PartyRepository(IServiceClient jsonServiceClient)
        {  
            _jsonServiceClient = jsonServiceClient;
        }

        ReadBranchResponse IPartyRepository.GetBranch(string searchString)
        {             
            ReadBranch request = new ReadBranch();
            request.PartyNumberOrCode = searchString;
            return _jsonServiceClient.Get(request);
        }
    }
}
