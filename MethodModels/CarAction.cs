using DrivingSchool.Context;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.MethodModels
{
    //Модель для работы с данными автомобиля
    public class CarAction
    {
        //ID автомобиля
        public int ID { get; set; }
        //Переменная EF Core для доступа к таблицам БД 
        public AppDbContext db { get; set; }
        public CarAction(int ID, AppDbContext db)
        {
            this.ID = ID;
            this.db = db;
        }
    }
}
