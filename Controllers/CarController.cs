using System.Linq;
using System.Threading.Tasks;
using DrivingSchool.Context;
using DrivingSchool.MethodModels;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.Controllers
{
    //Требуется Авторизация по ролям 
    [Authorize(Roles="Admin,User")]
    public class CarController : Controller
    {
        private readonly AppDbContext db;
        public CarController(AppDbContext context)
        {
            db = context;
        }

        //Данный метод используется при нажатии кнопок на View с данными пользователей
        //Метод нужен для распределения методов контроллера строго по кнопкам, нажатым на View
        public IActionResult MethodSwitcher(string button, int [] IDs, int ID)
        {
            if(IDs.Length != 0) 
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Car", new { IDs });
                }
            }else{
                if (button.Equals("Update"))
                {
                    if(ID > 0) {
                        return RedirectToAction("Update", "Car", ID);
                    }
                }else if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Car");
                }
            }
            return RedirectToAction("GetCars", "Car");
        }

        //Переход в View для вывода всех авто из БД
        [HttpGet]
        public IActionResult GetCars() => View(db);
        //Переход в View для создания записи автомобиля в БД
        public IActionResult Create() => View("CreateCar", db);
        //Создание записи автомобиля по введенных данным на View
        [HttpPost] 
        public async Task Create(Car car)
        {
            await db.Cars.AddAsync(car);
            await db.SaveChangesAsync();
        }

        //Переход в View для обновления записи об автомобиле в БД
        public IActionResult Update(int ID) => View("UpdateCar", new CarAction(ID, db));
        //Обновление записи автомобиля по введенных данным на View
        public async Task UpdatePost(Car car)
        {
            db.Cars.Update(car);
            await db.SaveChangesAsync();
        }

        //Удаление выбранных записей автомобилей по отмеченным ID
        public async Task<IActionResult> Delete(int [] IDs)
        {
            for(int i = 0; i < IDs.Count(); i++)
            {
                db.Cars.Remove(db.Cars.Find(IDs[i]));
                await db.SaveChangesAsync();
            }      
            return RedirectToAction("GetCars", "Car");
        }
    }
}
