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
    }
}
