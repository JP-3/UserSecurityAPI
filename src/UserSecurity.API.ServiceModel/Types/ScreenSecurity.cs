using System.Collections.Generic;

namespace UserSecurity.API.ServiceModel.Types
{
    public class ScreenSecurity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Default { get; set; }

        public string AccessLevel { get; set; }

        public string Sponsor { get; set; }

        public int SponsorScreenApproval { get; set; }

        public List<SecurityLevel> SecurityLevels { get; set; }
    }
}
