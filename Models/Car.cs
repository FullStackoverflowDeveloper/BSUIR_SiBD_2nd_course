using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.Models
{
    public class Car
    {
        [Key]
        public int ID { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int InstructorID { get; set; }
        [ForeignKey("InstructorID")]
        public Instructor Instructor { get; set; }
    }
}
