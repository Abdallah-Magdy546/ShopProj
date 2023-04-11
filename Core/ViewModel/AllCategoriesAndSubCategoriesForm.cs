using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class AllCategoriesAndSubCategoriesForm
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<string> SubCatsNames { get; set; }
    }
}
