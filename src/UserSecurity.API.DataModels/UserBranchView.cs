using System;

namespace UserSecurity.API.DataModels
{
   public class UserBranchView
    {
        public string Branch { get; set; }

        public bool Active { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool EditMoney { get; set; }
    }
}
