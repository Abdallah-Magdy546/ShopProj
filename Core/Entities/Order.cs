using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Product))]
        public string UserId { get; set; }
        public int ProductId { get; set;}
        public int Quantity { get; set; }
        public string status { get; set; }
    }
}
