using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.Models
{
    public class Exam
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateExam { get; set; }
        public int StudentID { get; set; }
        [ForeignKey("StudentID")]
        public Student Student { get; set; }
        public int CarID { get; set; }
        [ForeignKey("CarID")]
        public Car Car { get; set; }
        public int InstructorID { get; set; }
        [ForeignKey("InstructorID")]
        public Instructor Instructor { get; set; }
        public string TraffPoliceDep { get; set; }
    }
}
