using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            return View(roleManager.Roles);
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = id,
                RoleName = role.Name,
            };

            //Kudvenkat's code does: it works, by set Multiple Active Result (MARS) in connectionString to true or ToList() userManager.Users
            //foreach (var user in userManager.Users)
            //{
            //    if (await userManager.IsInRoleAsync(user, role.Name))
            //    {
            //        model.Users.Add(user.UserName);
            //    }
            //}

            //Netizen's code does :D
            IList<ApplicationUser> listUsers = await userManager.GetUsersInRoleAsync(role.Name);
            foreach (var item in listUsers)
            {
                model.Users.Add(item.UserName);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            role.Name = model.RoleName;

            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            List<UserRoleViewModel> model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                UserRoleViewModel item = new UserRoleViewModel();
                item.UserId = user.Id;
                item.UserName = user.UserName;
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }

                model.Add(item);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"The Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            IdentityResult result = new IdentityResult();
            foreach (var item in model)
            {

                ApplicationUser user = await userManager.FindByIdAsync(item.UserId);
                bool status = await userManager.IsInRoleAsync(user, role.Name);
                if (item.IsSelected && !status)
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!item.IsSelected && status)
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }

            //for (int i = 0; i< model.Count; i++)
            //{
            //    IdentityResult result = new IdentityResult();
            //    ApplicationUser user = await userManager.FindByIdAsync(model[i].UserId);

            //    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
            //    {
            //        result = await userManager.AddToRoleAsync(user, role.Name);
            //    }
            //    else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
            //    {
            //        result = await userManager.RemoveFromRoleAsync(user, role.Name);
            //    }
            //    else
            //    {
            //        continue;
            //    }
            //    if (result.Succeeded)
            //    {
            //        if (i<(model.Count-1))
            //        {
            //            continue;   
            //        }
            //        else
            //        {
            //            return RedirectToAction("EditRole", new {id = roleId});
            //        }
            //    }
            //}

            return RedirectToAction("EditRole", new { id = roleId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            EditUserViewModel model = new EditUserViewModel();
            model.Id = user.Id;
            model.Email = user.Email;
            model.UserName = user.UserName;
            model.City = user.City;
            model.Roles = userRoles;
            model.Claims = userClaims.Select(x => x.Value).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.City = model.City;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}
