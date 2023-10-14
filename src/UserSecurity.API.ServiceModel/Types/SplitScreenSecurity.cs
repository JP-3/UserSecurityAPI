using System.Collections.Generic;

namespace UserSecurity.API.ServiceModel.Types
{
    public class SplitScreenSecurity
    {
        public List<ScreenSecurity> StandardScreenSecurity { get; set; }

        public List<ScreenSecurity> SponsorScreenSecurity { get; set; }
    }
}
