using DrivingSchool.Context;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DrivingSchool.MethodModels
{
    public class StudentAction
    {
        [Key]
        public int ID { get; set; }
        public string Passport { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string GroupNumb { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public AppDbContext db { get; set; }

        public StudentAction(Student stud, IEnumerable<Group> groups)
        {
            Groups = groups;
            ID = stud.ID;
            Passport = stud.Passport;
            FName = stud.FName;
            MName = stud.MName;
            LName = stud.LName;
            Address = stud.Address;
            GroupNumb = stud.GroupNumb;
        }

        public StudentAction(AppDbContext context)
        {
            db = context;
        }
    }
}
