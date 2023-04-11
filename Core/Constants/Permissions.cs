using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string Module)
        {
            return new List<string>
            {
                $"Permissions.{Module}.View",
                $"Permissions.{Module}.Create",
                $"Permissions.{Module}.Edit",
                $"Permissions.{Module}.Delete"

            };
        }
        public static List<string> GenerateAllPermissions()
        {
       
                var allPermissions = new List<string>();
                var modules = Enum.GetValues(typeof(Modules)).Cast<Modules>();
                foreach (var module in modules)
                {
                    allPermissions.AddRange(GeneratePermissionsList(module.ToString()));
                }
                return allPermissions;
        }
        public static class Students
        {
            public const string View = "Permissions.Students.View";
            public const string Create = "Permissions.Students.Create";
            public const string Edit = "Permissions.Students.Edit";
            public const string Delete = "Permissions.Students.Delete";
        }
        public static class Categories

        {
            public const string View = "Permissions.Categories.View";
            public const string Create = "Permissions.Categories.Create";
            public const string Edit = "Permissions.Categories.Edit";
            public const string Delete = "Permissions.Categories.Delete";
        }
        public static class SubCategories
        {
            public const string View = "Permissions.SubCategories.View";
            public const string Create = "Permissions.SubCategories.Create";
            public const string Edit = "Permissions.SubCategories.Edit";
            public const string Delete = "Permissions.SubCategories.Delete";
        }
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
    }
}
