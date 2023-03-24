using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationProj.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(AllowEmptyStrings =true)]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [Required]  
        public string GENDER { get; set; }
    }
}
