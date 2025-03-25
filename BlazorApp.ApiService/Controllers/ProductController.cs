using BlazorApp.BL.Services;
using BlazorApp.Common.Resources;
using BlazorApp.Model.Entities;
using BlazorApp.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BlazorApp.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, IStringLocalizer<ProductTranslation> localizer, ILogger<ProductController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetProducts()
        {
            try
            {
                logger.LogInformation("Test log GetProducts");
                var products = await productService.GetProducts();
                foreach (var product in products)
                {
                    product.Description = product.Description != null ? localizer[product.Description] : null;
                }
                return Ok(new BaseResponseModel { Success = true, Data = products });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetProducts error");
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel productModel)
        {
            await productService.CreateProduct(productModel);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetProduct(int id)
        {
            var productModel = await productService.GetProduct(id);

            if (productModel == null)
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }
            return Ok(new BaseResponseModel { Success = true, Data = productModel });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductModel productModel)
        {
            if (id != productModel.ID || !await productService.ProductModelExists(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
            }

            await productService.UpdateProduct(productModel);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await productService.ProductModelExists(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }
            await productService.DeleteProduct(id);
            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
