using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class SubCategoriesAndCategoriesEditForm
    {
        public int id { get; set; }
        public string name { get; set; }
        public string CatName { get; set; }
        public List<string> CatsNames { get; set; }
    }
}
