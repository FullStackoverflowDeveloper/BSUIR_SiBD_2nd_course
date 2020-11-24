using DrivingSchool.Context;
using DrivingSchool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.MethodModels
{
    public class ExamAction
    {
        [Key]
        public int ID { get; set; }
        public AppDbContext db { get; set; }

        public ExamAction(int ID,AppDbContext db)
        {
            this.ID = ID;
            this.db = db;
        }
    }
}
