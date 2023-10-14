using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSecurity.API.ServiceModel.Types
{
    public class UserCertification
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
