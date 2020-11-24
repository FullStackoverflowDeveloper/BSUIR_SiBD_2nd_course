using System;
using System.Threading.Tasks;
using DrivingSchool.Globals;
using DrivingSchool.Models;
using DrivingSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.Controllers
{
    //Контроллер, отвечающий за аутентификацию и авторизацию
    public class EntranceController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public EntranceController(UserManager<User> usrMgr, SignInManager<User> signinMgr)
        {
            userManager = usrMgr;
            signInManager = signinMgr;
        }

        //Переход на регистрационную View
        [HttpGet]
        public IActionResult Register() => View();

        //Метод для работы с данными, введенными пользователем
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //Проверка на правильность и наличие ввееденных данных
            if (ModelState.IsValid)
            {
                //Присваивание отдельных полей в модель регистрации
                User user = new User
                {
                    UserName = model.Name,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now.ToString(),
                    LatestLoginDate = DateTime.Now.ToString(),
                    Status = true
                };

                //Создание аккаунта пользователя
                var result = await userManager.CreateAsync(user, model.Password);
                //Проверка результата создания аккаунта
                if (result.Succeeded)
                {
                    //Присвоение роли новому пользователю
                    var res = await userManager.AddToRoleAsync(user, "User");
                    //Проверка результата присвоения роли
                    if (res.Succeeded)
                    {
                        //Аутентификация, после регистрации
                        await signInManager.SignInAsync(user, false);
                        //Переход в аккаунт с доступом к данным по роли "User"
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Действия при провале действия создания аккаунта
                else
                {
                    //Добавление ошибок регистрации на View
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            //Переход на View c перечнем ошибок в веденных данных пользователем
            return View(model);
        }

        //Метод для перхеода на View логина пользователя
        public ViewResult Login(string returnUrl = null)
        {
            //Переход на View логина
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        //Работа с данными для логина, введенными пользователем
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //Проверка на правильность введенных данных
            if (ModelState.IsValid)
            {
                //Поиск данных пользователя
                User user = await userManager.FindByEmailAsync(model.Email);
                //Проверка введенных данных на существование аккаунта
                if (user != null)
                {
                    //Проверка аккаунта пользователя на блокировку
                    bool blockResult = EntranceProcessing.CheckBlock(user);
                    //При отсутствующей блокировке
                    if (!blockResult)
                    {
                        //Аунтентификация
                        var result =
                            await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                        //При успешной аутентификации...
                        if (result.Succeeded)
                        {
                            //Обновления даты последнего входа в аккаунт для пользователя
                            await userManager.UpdateAsync(EntranceProcessing.UpdateUser(user));
                            //Проверка переменной на наличие данных в ней(не долнжо быть)
                            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                            //Результат успешной аутентификации
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        //При неудачной аутентификации
                        else
                        {
                            ModelState.AddModelError("", "Login Failed: Invalid Email and (or) password");
                        }
                    }
                    //При заблокированном аккаунте будет выведена соответствующая ошибка
                    else
                    {
                        ModelState.AddModelError("", "Login Failed: This account is blocked!");
                    }
                }
                //При несуществующем аккаунте будет выведена соответствующая ошибка
                else
                {
                    ModelState.AddModelError("", "This account doesn't exists!");
                }
            }
            //Переход на View c перечнем ошибок в веденных данных пользователем
            return View(model);
        }

        //Метод для выхода из аккаунта
        public async Task<IActionResult> Logout()
        {
            //Выход
            await signInManager.SignOutAsync();
            //Переброс на первичную страницу с общей информацией
            return RedirectToAction("Index", "Home");
        }
    }
}
