using DrivingSchool.Context;
using DrivingSchool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.MethodModels
{
    public class GroupAction
    {
        public int ID { get; set; }
        public AppDbContext db { get; set; }

        public GroupAction(int ID, AppDbContext db)
        {
            this.ID = ID;
            this.db = db;
        }

        public GroupAction(AppDbContext db)
        {
            this.db = db;
        }
    }
}
