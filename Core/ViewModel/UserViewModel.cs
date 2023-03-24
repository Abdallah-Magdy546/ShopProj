using System.ComponentModel.DataAnnotations;

namespace Core.ViewModel
{
    public class UserViewModel
    {
        public string id { get; set; }
        [Display(Name =("name"))]
        public string UserName { get; set; }
        [Display(Name = ("PhoneNum"))]
        public string PhoneNum { get; set; }
        [Display(Name = ("Gender"))]
        public string Gender { get; set; }
        public string Email { get; set; }
        [Display(Name = ("Roles"))]
        public IEnumerable<string> Roles { get; set; }
    }
}
