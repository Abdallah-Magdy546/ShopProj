using AuthenticationProj.ViewModel;
using Core.Constants;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthenticationProj.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;


        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> CreateNewRole(RoleFormViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            if (await _roleManager.RoleExistsAsync(role.name))
            {
                ModelState.AddModelError("name", "RoleIsExist");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole(role.name.Trim()));
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = _roleManager.Roles.Where(R => R.Id == id).FirstOrDefault();
            if (role == null)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            else
            {
                await _roleManager.DeleteAsync(role);
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

        }
        public async Task<IActionResult> ManagePermission(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = allClaims.Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue))
                    permission.IsSelected = true;
            }

            var viewModel = new PermissionsFormViewModel
            {
                RoleId = id,
                RoleName = role.Name,
                RoleClaims = allPermissions
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermission(PermissionsFormViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleClaims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction(nameof(Index));
        }
    }
}
