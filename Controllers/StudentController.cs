using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrivingSchool.Context;
using DrivingSchool.MethodModels;
using DrivingSchool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrivingSchool.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class StudentController : Controller
    {
        private readonly AppDbContext db;
        public StudentController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult MethodSwitcher(string button, int[] IDs, int ID)
        {
            if (IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Student", new { IDs });
                }
            }
            else
            {
                if (button.Equals("Update"))
                {
                    if (ID > 0)
                    {
                        return RedirectToAction("Update", "Student", ID);
                    }
                }
                else if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Student");
                }
            }
            return RedirectToAction("GetStudents", "Student");
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            StudentAction stud = new StudentAction(db);
            return View(stud);
        }

        public IActionResult Create() {
            IEnumerable<Group> groups = db.Groups.ToList();
            return View("CreateStudent", groups); 
        }

        [HttpPost]
        public async Task Create(Student student)
        {
            await db.Students.AddAsync(student);
            await db.SaveChangesAsync();
        }

        public IActionResult Update(int ID)
        {
            StudentAction student = new StudentAction(db.Students.Find(ID), db.Groups.ToList());
            return View("UpdateStudent", student);
        }

        public async Task UpdatePost(Student student)
        {
            db.Students.Update(student);
            await db.SaveChangesAsync();
        }

        public async Task<IActionResult> Delete(int[] IDs)
        {
            for (int i = 0; i < IDs.Count(); i++)
            {
                Student student = await db.Students.FindAsync(IDs[i]);
                db.Students.Remove(student);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GetStudents", "Student");
        }
    }
}
