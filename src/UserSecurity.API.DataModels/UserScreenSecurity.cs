namespace UserSecurity.API.DataModels
{
    public class UserScreenSecurity
    {
        public int ScreenID { get; set; }

        public string ScreenName { get; set; }

        public int SecStyle { get; set; }

        public string LevelDescription { get; set; }

        public int PasswordLevel { get; set; }

        public bool PasswordRequired { get; set; }

        public int AccessLevel { get; set; }
    }
}
