using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(256)]
        public string Password { get; set; }
        [Required, MaxLength(10)]
        public string Gender { get; set; }
    }
}
