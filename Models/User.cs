using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.Models
{
    public class User : IdentityUser
    {
        public string RegistrationDate { get; set; }
        public string LatestLoginDate { get; set; }
        public bool Status { get; set; }
        public bool Check { get; set; }
    }
}
