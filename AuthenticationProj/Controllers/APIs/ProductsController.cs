using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopProj.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductInterface _products;
        private readonly ISubCategoryInterface _subCategory;

        public ProductsController(IProductInterface Products , ISubCategoryInterface subCategory)
        {
            _products = Products;
            _subCategory = subCategory;
        }
        [HttpPost]
        public async Task<IActionResult> GetProductsFilteredAndPaged()
        {
            //var SubCategory = await _subCategory.GetSubCategoryByName(name);
            //var SubCategoryId = SubCategory.id;
            int pageSize = int.Parse(Request.Form["length"]);
            int skip = int.Parse(Request.Form["start"]);
            var SubCategory = await _subCategory.GetSubCategoryByName("Cars");
            var SearchValue = Request.Form["search[value]"];
            //var products = await _products.GetAllProductsBySubCategoryIdPaging(SubCategory.id ,SearchValue);
            var products = await _products.GetAllProductsBySubCategoryId(SubCategory.id);
            var ProductWithSearch = products.Where(m => string.IsNullOrEmpty(SearchValue)
            ? true
            : (m.name.Contains(SearchValue) || m.model.Contains(SearchValue) || m.SellerName.Contains(SearchValue)
            || m.brand.Contains(SearchValue)));
            var data = ProductWithSearch.Skip(skip).Take(pageSize).ToList();
            var recordsTotal = products.Count();
            var jsonData = new {recordsFiltered= recordsTotal, recordsTotal , data};
            return Ok (jsonData);
        }
    }
}
