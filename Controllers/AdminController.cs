using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeetCarAccidents.Models;

namespace YeetCarAccidents.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        //VIEW ALL USERS
        [Route("Admin/ListUsers")]
        [HttpGet]
        [Authorize(Roles ="Writer")]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        //VIEW ALL ROLES
        [Route("Admin/ListRoles")]
        [HttpGet]
        [Authorize(Roles = "Writer")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        //CREATE ROLES
        [HttpGet]
        [Route("Admin/CreateRole")]
        [Authorize(Roles = "Writer")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Route("Admin/CreateRole")]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRole(UserRoleView role)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.RoleName);
            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role.RoleName));
            }
            return View();
        }
        //CHANGE ROLES OF USERS
        [Route("Admin/EditUsersInRole")]
        [HttpGet]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditUsersInRole(string Id)
        {
            ViewBag.roleId = Id;
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {Id} is not found";
                return View("NotFound");
            }

            var model = new List<UserModel>();
            foreach (var user in userManager.Users)
            {
                var UserRoleView = new UserModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRoleView.IsSelected = true;
                }
                else
                {
                    UserRoleView.IsSelected = false;
                }
                model.Add(UserRoleView);
            }
            return View(model);
        }
        [Route("Admin/EditUsersInRole")]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditUsersInRole(List<UserModel> model, string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role id {Id} is not found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = Id });
                }
            }
            return RedirectToAction("EditRole", new { Id = Id });
        }
    }
}
