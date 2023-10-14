namespace UserSecurity.API.DataModels
{
    public class ScreenAccessLevel
    {
        public int ScreenID { get; set; }

        public string ScreenName { get; set; }

        public int AccessLevel { get; set; }

        public string LevelDescription { get; set; }

        public string Notes { get; set; }
    }   
}
