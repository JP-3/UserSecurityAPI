
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserSecurity.API.DataModels
    {
        public class UserCerts
        {
            public int EmpTrainingID { get; set; }

            public string EmpCode { get; set; }

            public int CourseID { get; set; }

            public DateTime DatePassed { get; set; }

            public DateTime ExpirationDate { get; set; }

            public DateTime EnteredDate { get; set; }

            public string EnteredBy { get; set; }
        }
    }


