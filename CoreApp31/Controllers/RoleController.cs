using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApp31.Data;
using CoreApp31.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreApp31.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        // inject the RoleManager to Create/List roles
        private readonly RoleManager<IdentityRole> roleManager;      
        private readonly UserManager<IdentityUser> userManager;
   
        public RoleController(RoleManager<IdentityRole> roleManager, VodafoneWebAuthDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;          
            this.userManager = userManager;
        }

        [Authorize(Policy = "Adminpolicy")]
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [Authorize(Policy = "Adminpolicy")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public IActionResult Create(IdentityRole role)
        {
            var res = roleManager.CreateAsync(role).Result;
            return RedirectToAction("Index");
        }
        [Authorize(Policy = "Adminpolicy")]
        public async Task<IActionResult> UserList()
        {
            var users = userManager.Users.ToList();
            List<UserRoleViewModel> userrolemodel = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                string role = roles.FirstOrDefault();
                userrolemodel.Add(new UserRoleViewModel { UserId = user.Id, UserName = user.UserName, RoleName = role });

            }

            return View(userrolemodel);
        }

        [Authorize(Policy = "Adminpolicy")]
        public IActionResult AssignRole(string id)
        {
            var user=userManager.FindByIdAsync(id);
            var userrole = userManager.GetRolesAsync(user.Result);
            string role = string.Empty;
            if (userrole.Result != null && userrole.Result.Count() > 0)
            {

                role = userrole.Result.FirstOrDefault();

            }
            ViewBag.users = userManager.Users.ToList();
            ViewBag.roles = roleManager.Roles.ToList();
            return View(new UserRoleViewModel {UserName=user.Result.UserName,RoleName= role }); ;
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(UserRoleViewModel model)
        {
            var role = roleManager.FindByNameAsync(model.RoleName);
            var user = userManager.FindByNameAsync(model.UserName).Result;

            await userManager.AddToRoleAsync(user, model.RoleName);
            return RedirectToAction("UserList");
        }

    }
}
