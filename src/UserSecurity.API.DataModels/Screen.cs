namespace UserSecurity.API.DataModels
{
    public class Screen
    {
        public int ScreenID { get; set; }

        public string ScreenName { get; set; }

        public int AccessLevelDefault { get; set; }

        public string Sponsor { get; set; }

        public int SponsorApprovalLevel { get; set; }
    }
}
