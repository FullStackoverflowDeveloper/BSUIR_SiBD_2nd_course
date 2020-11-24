using DrivingSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingSchool.MethodModels
{
    //Модель работы с ролями для пользователей
    public class RoleChanger
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<User> UsersAll { get; set; }
        public IList<User> UsersInRole { get; set; }
        public RoleChanger()
        {
            UsersAll = new List<User>();
            UsersInRole = new List<User>();
        }
    }
}
