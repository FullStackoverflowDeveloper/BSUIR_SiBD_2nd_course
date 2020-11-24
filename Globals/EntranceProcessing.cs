using DrivingSchool.Models;
using System;

namespace DrivingSchool.Globals
{
    public class EntranceProcessing
    {
        //Обновление значения переменной
        //(а в последствии столбца в БД в кортеже) аутентифицириемого пользователя
        public static User UpdateUser(User user)
        {
            user.LatestLoginDate = DateTime.Now.ToString();
            return user;
        }

        //Проверка блокировки на аутентифицируемом пользователе
        public static bool CheckBlock(User user)
        {
            bool result;
            if (user.Status)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
