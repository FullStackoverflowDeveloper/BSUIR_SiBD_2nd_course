using DrivingSchool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.MethodModels
{
    public class InstructorAction
    {
        [Key]
        public int ID { get; set; }
        public string Passport { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }

        public InstructorAction(Instructor instr)
        {
            ID = instr.ID;
            Passport = instr.Passport;
            FName = instr.FName;
            MName = instr.MName;
            LName = instr.LName;
            Address = instr.Address;
        }
    }
}
