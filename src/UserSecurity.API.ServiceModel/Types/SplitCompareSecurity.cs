using System.Collections.Generic;

namespace UserSecurity.API.ServiceModel.Types
{
    public class SplitCompareSecurity
    {
        public List<CompareSecurity> StandardScreenSecurity { get; set; }

        public List<CompareSecurity> SponsorScreenSecurity { get; set; }
    }
}
