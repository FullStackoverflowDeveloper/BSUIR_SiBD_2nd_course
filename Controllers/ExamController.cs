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
    public class ExamController : Controller
    {
        private readonly AppDbContext db;
        public ExamController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult MethodSwitcher(string button, int[] IDs, int ID)
        {
            if (IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Exam", new { IDs });
                }
            }
            else
            {
                if (button.Equals("Update"))
                {
                    if (ID > 0)
                    {
                        return RedirectToAction("Update", "Exam", ID);
                    }
                }
                else if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Exam");
                }
            }
            return RedirectToAction("GetExams", "Exam");
        }

        [HttpGet]
        public IActionResult GetExams() => View("GetExams", db);

        public IActionResult Create() => View("CreateExam", db);

        [HttpPost]
        public async Task Create(Exam exam)
        {
            await db.Exams.AddAsync(exam);
            await db.SaveChangesAsync();
        }

        public IActionResult Update(int ID) => View("UpdateExam", new ExamAction(ID, db));

        public async Task UpdatePost(Exam exam)
        {
            db.Exams.Update(exam);
            await db.SaveChangesAsync();
        }

        public async Task<IActionResult> Delete(int[] IDs)
        {
            for (int i = 0; i < IDs.Count(); i++)
            {
                db.Exams.Remove(db.Exams.Find(IDs[i]));
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GetExams", "Exam");
        }
    }
}
