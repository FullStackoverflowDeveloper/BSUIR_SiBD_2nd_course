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
    public class InstructorController : Controller
    {
        private readonly AppDbContext db;
        public InstructorController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult MethodSwitcher(string button, int[] IDs, int ID)
        {
            if (IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Instructor", new { IDs });
                }
            }
            else
            {
                if (button.Equals("Update"))
                {
                    if (ID > 0)
                    {
                        return RedirectToAction("Update", "Instructor", ID);
                    }
                }
                else if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Instructor");
                }
            }
            return RedirectToAction("GetInstructors", "Instructor");
        }

        [HttpGet]
        public IActionResult GetInstructors()
        {
            IEnumerable<Instructor> instructors = db.Instructors.ToList();
            return View(instructors);
        }

        public IActionResult Create() => View("CreateInstructor");

        [HttpPost]
        public async Task Create(Instructor instructor)
        {
            await db.Instructors.AddAsync(instructor);
            await db.SaveChangesAsync();
        }

        public IActionResult Update(int ID)
        {
            InstructorAction instructor = new InstructorAction(db.Instructors.Find(ID));
            return View("UpdateInstructor", instructor);
        }

        public async Task UpdatePost(Instructor instructor)
        {
            db.Instructors.Update(instructor);
            await db.SaveChangesAsync();
        }

        public async Task<IActionResult> Delete(int[] IDs)
        {
            for (int i = 0; i < IDs.Count(); i++)
            {
                Instructor instructor = await db.Instructors.FindAsync(IDs[i]);
                db.Instructors.Remove(instructor);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GetInstructors", "Instructor");
        }
    }
}
