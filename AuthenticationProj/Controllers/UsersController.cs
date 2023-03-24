using AuthenticationProj.Models;
using AuthenticationProj.ViewModel;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthenticationProj.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.Select(u => new UserViewModel
            {
                id = u.Id,
                UserName = u.UserName,
                PhoneNum = u.PhoneNumber,
                Gender = u.GENDER,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result
            }).ToListAsync();

            return View(users);
        }
        public async Task<IActionResult> EditUser(string id)
        {
            var User = await _userManager.FindByIdAsync(id);
            if (User == null) { return NotFound(); }
            var roles = await _roleManager.Roles.ToListAsync();
            var ViewModel = new UserRolesViewModel
            {
                UserId = User.Id,
                UserName = User.UserName,
                Roles = roles.Select(R => new CheckBoxViewModel
                {
                    RoleId = R.Id,
                    DisplayValue = R.Name,
                    IsSelected = _userManager.IsInRoleAsync(User, R.Name).Result
                }).ToList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditUser(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();
            var UserRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if(UserRoles.Any(r=>r==role.DisplayValue)&& !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.DisplayValue);
                
                if (!UserRoles.Any(r => r == role.DisplayValue) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.DisplayValue);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager?.FindByIdAsync(id);
            if (user == null) return NotFound();
            else { await _userManager.DeleteAsync(user); }
            return RedirectToAction(nameof(Index));
        }
    }
}
