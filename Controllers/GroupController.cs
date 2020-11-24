using System;
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
    public class GroupController : Controller
    {
        private readonly AppDbContext db;
        public GroupController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult MethodSwitcher(string button, int[] IDs, int ID)
        {
            if (IDs.Length != 0)
            {
                if (button.Equals("Delete"))
                {
                    return RedirectToAction("Delete", "Group", new { IDs });
                }
            }
            else
            {
                if (button.Equals("Update"))
                {
                    if (ID > 0)
                    {
                        return RedirectToAction("Update", "Group", ID);
                    }
                }
                else if (button.Equals("Create"))
                {
                    return RedirectToAction("Create", "Group");
                }
            }
            return RedirectToAction("GetGroups", "Group");
        }

        [HttpGet]
        public IActionResult GetGroups() => View("GetGroups", db);

        public IActionResult Create() => View("CreateGroup", db);

        [HttpPost]
        public async Task Create(Group group)
        {
            await db.Groups.AddAsync(group);
            await db.SaveChangesAsync();
        }

        public IActionResult Update(int ID) => View("UpdateGroup", new GroupAction(ID, db));

        public async Task UpdatePost(Group group)
        {
            db.Groups.Update(group);
            await db.SaveChangesAsync();
        }

        public async Task<IActionResult> Delete(int[] IDs)
        {
            for (int i = 0; i < IDs.Count(); i++)
            {
                Group group = await db.Groups.FindAsync(IDs[i]);
                db.Groups.Remove(group);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("GetGroups", "Group");
        }
    }
}
