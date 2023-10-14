using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserSecurity.API.DataModels
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
    }
}
