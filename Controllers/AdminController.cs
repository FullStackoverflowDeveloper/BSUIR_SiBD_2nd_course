using System.Linq;
using System.Threading.Tasks;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.Controllers
{
    //Требуется роль "Admin" для работы с контроллером
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //Переменная EF Core для работы с данными в БД
        private readonly UserManager<User> userManager;
        public AdminController(UserManager<User> usrMgr)
        {
            userManager = usrMgr;
        }

        //Переход на View c выводом данных с пользователях
        public IActionResult GetUsers() => View(userManager.Users);

        //Данный метод используется при нажатии кнопок на View с данными пользователей
        //Метод нужен для распределения методов контроллера строго по кнопкам, нажатым на View
        public IActionResult MethodSwitcher(string button, string[] IDs)
        {
            if(IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Admin", new { IDs });
                }
                else
                {
                    if (button.Equals("Block"))
                        return RedirectToAction("Block", "Admin", new { IDs });
                    else if (button.Equals("Unblock"))
                        return RedirectToAction("Unblock", "Admin", new { IDs });
                    else if (button.Equals("Delete"))
                        return RedirectToAction("Delete", "Admin", new { IDs });
                }
            }
            return View("GetUsers", userManager.Users);
        }
        //Метод блокировки пользователей
        public async Task<IActionResult> Block(string[] IDs)
        {
            //По количеству ID пользователей цикл
            for (int i = 0; i < IDs.Count(); i++)
            {
                //Поиск пользователя по ID
                User usr = await userManager.FindByIdAsync(IDs[i]);
                //Блокировка пользователя
                usr.Status = false;
                //Обновление данных о пользователе в БД
                await userManager.UpdateAsync(usr);
            }
            //Находим информацию о аутентифицированном пользователе
            User userBlock = await userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User));
            //Проверяем ID аутентифицированного пользователя и выходим
            //, если он в списке заблокированных
            if (userBlock.Status == false)
            {
                return RedirectToAction("Logout", "Entrance");
            }
            //Возврат к таблице пользователей
            return View("GetUsers", userManager.Users);
        }
        //Метод разблокировки пользователей
        public async Task<IActionResult> UnBlock(string[] IDs)
        {
            //По количеству ID пользователей цикл
            for (int i = 0; i < IDs.Count(); i++)
            {
                //Поиск пользователя по ID
                User usr = await userManager.FindByIdAsync(IDs[i]);
                //Разблокировка пользователя
                usr.Status = true;
                //Обновление данных о пользователе в БД
                await userManager.UpdateAsync(usr);
            }
            //Возврат к таблице пользователей
            return View("GetUsers", userManager.Users);
        }
        //Метод удаления пользователей
        public async Task<IActionResult> Delete(string[] IDs)
        {
            //По количеству ID пользователей цикл
            for (int i = 0; i < IDs.Count(); i++)
            {
                //Поиск пользователя по ID
                User usr = await userManager.FindByIdAsync(IDs[i]);
                //Удаление пользователя из БД
                await userManager.DeleteAsync(usr);
            }
            //Получение ID аутентифицированного пользователя
            string signeduserId = userManager.GetUserId(HttpContext.User);
            //По циклу ID пользователей проверяем ID аутентифицированного пользователя и выходим
            //, если он в списке удалённых
            for (int i = 0; i < IDs.Count(); i++)
            {
                if (IDs[i].Equals(signeduserId)) { return RedirectToAction("Logout", "Entrance"); }
            }
            //Возврат к таблице пользователей
            return View("GetUsers", userManager.Users);
        }
    }
}
