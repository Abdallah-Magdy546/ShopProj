using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SubCategory
    {
        [Key]
        public int id { get; set; }
        [Required,StringLength(50),Display(Name = "name")]
        public string Name { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        public byte[] Photo { get; set; }
    }
}
