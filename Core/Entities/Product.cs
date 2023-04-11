using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        [Required , Display(Name ="name"),StringLength(50)]
        public string name { get; set; }
        [Required, Display(Name = "brand"), StringLength(50)]
        public string brand { get; set; }
        [Required, Display(Name = "model"), StringLength(100)]
        public string model { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime ProductionDate { get; set; }
        [ForeignKey(nameof(SubCategory))]
        public int SubCategoryId { get; set; }
        [Required]
        public float price { get; set; }
        [Required,StringLength(50)]
        public String SellerName { get; set; }
    }
}
