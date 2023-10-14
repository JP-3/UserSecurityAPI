using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSecurity.API.Interfaces.Repositories;
using UserSecurity.API.Interfaces.Managers;
using PartyV5.ServiceModel.Messages;

namespace UserSecurity.API.Managers
{
    public class PartyManager : IPartyManager
    {
        private readonly IPartyRepository _partyRepository;

        public PartyManager(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        ReadBranchResponse IPartyManager.GetBranch(string searchString)
        {
            return _partyRepository.GetBranch(searchString);
        }
    }
}
