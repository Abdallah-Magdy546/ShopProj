using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class student
    {
        [Display(Name ="id")]
        public int id { get; set; }
        [Display(Name = "name")]
        public string name { get; set; }
        [Display(Name = "description")]
        public string description { get; set; }

    }
}
