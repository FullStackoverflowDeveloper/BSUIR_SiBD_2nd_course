using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrivingSchool.MethodModels;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.Controllers
{
    //Требуется роль "Admin" для работы с контроллером
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        //Переменные EF Core для работы с данными в БД
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public RoleController(RoleManager<IdentityRole> rlManager, UserManager<User> usrManager)
        {
            roleManager = rlManager;
            userManager = usrManager;
        }

        //Данный метод используется при нажатии кнопок на View с данными пользователей
        //Метод нужен для распределения методов контроллера строго по кнопкам, нажатым на View
        public IActionResult MethodSwitcher(string button, string[] IDs)
        {
            if (IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Role", new { IDs });
                }
            }
            else
            {
                if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Role");
                }
            }
            return View("GetRoles", roleManager.Roles);
        }
        //Получение всех ролей на View 
        public IActionResult GetRoles() {
            IEnumerable<IdentityRole> roles = roleManager.Roles;
            return View("GetRoles", roles); 
        }

        //Переход на View для создания роли
        public IActionResult Create() => View("Create");

        //Обработка введенных данных на создаваемую роль
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            //Проверка на наличие данных
            if (!string.IsNullOrEmpty(name))
            {
                //Создание записи в таблице в БД
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    //При успешном оздании вывод всех ролей
                    return RedirectToAction("GetRoles", roleManager.Roles);
                }
                else
                {
                    //Вывод ошибок на View
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            //Передача ошибок, при пустом вводе
            return View(name);
        }

        //Удаление Ролей из БД по ID
        public async Task<IActionResult> Delete(string[] IDs)
        {
            //Проход по массиву ID
            for (int i = 0; i < IDs.Count(); i++)
            {
                //Поиск по ID роли в БД
                IdentityRole role = await roleManager.FindByIdAsync(IDs[i]);
                //Удаление роли из БД
                await roleManager.DeleteAsync(role);
            }
            //Возврат к View с таблицей ролей
            return View("GetRoles", roleManager.Roles);
        }

        //Метод выводит пользователей для выбранной роли
        public async Task<IActionResult> UpdateRole(string roleId)
        {
            //Поиск роли по ID
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            //Проверка на наличие роли
            if (role != null)
            {
                //Получаем пользователей, относящихся к роли
                var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                //Получаем всех пользователей
                var usersAll = userManager.Users.ToList();
                //Формируем объект роли на вывод
                RoleChanger model = new RoleChanger
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    UsersAll = usersAll,
                    UsersInRole = usersInRole
                };
                //Переход на View с передачей ообъекта
                return View("UpdateRole", model);
            }
            //При пустой переменной выводим соответствующую ошибку
            return NotFound();
        }

        //Метод обновляет пользователей для выбранной роли
        [HttpPost]
        public async Task<IActionResult> UpdatePost(string roleId, List<string> users)
        {
            //Поиск роли в БД
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            //Проверка на наличие роли
            if (role != null)
            {
                //Получаем пользователей, относящихся к роли
                var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                //Создание коллекции пользователей, а затем заполнение её определенными на View пользователями
                List<User> usersList = new List<User>();
                foreach (var user in users)
                {
                    //Поиск пользователей по ID
                    var userResult = await userManager.FindByIdAsync(user);
                    //Добавление пользователей в коллекцию
                    usersList.Add(userResult);
                }

                /*Далее написаны операции для получения списков пользователей
                *путём исключения
                */
                var addedUsers = usersList.Except(usersInRole);
                var removedUsers = usersInRole.Except(usersList);
                foreach (User user in addedUsers)
                {
                    //Добавление юзера в роль
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                foreach (User user in removedUsers)
                {
                    //Удаления юзера из роли
                    await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                //Переход к View для вывода ролей
                return View("GetRoles", roleManager.Roles.ToList());
            }
            //При пустой переменной выводим соответствующую ошибку
            return NotFound();
        }
    }
}
