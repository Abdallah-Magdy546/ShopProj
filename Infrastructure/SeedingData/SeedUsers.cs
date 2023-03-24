using AuthenticationProj.Models;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedingData
{
    public static class SeedUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "AbdallahMagdy",
                Email = "AbdallahMagdy@yahoo.com",
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser,"Aa@123");
                await userManager.AddToRoleAsync(defaultUser,Roles.Basic.ToString());
            }
        }
        public static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "Admin@yahoo.com",
                EmailConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Aa@123");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Basic.ToString(), Roles.Admin.ToString(), Roles.SuperAdmin.ToString() });
            }
            await roleManager.SeedClaimsForSuperUser();
        }
        private static async Task SeedClaimsForSuperUser (this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaims(adminRole, "Product");
        }
        public static async Task AddPermissionClaims(this RoleManager<IdentityRole> roleManager,IdentityRole role, string Module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList(Module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c=>c.Type == "Permission" && c.Value==permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
 