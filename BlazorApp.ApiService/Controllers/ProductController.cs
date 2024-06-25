using BlazorApp.BL.Services;
using BlazorApp.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetProducts()
        {
            var products = await productService.GetProducts();
            return Ok(new BaseResponseModel { Success = true, Data = products });
        }
    }
}
