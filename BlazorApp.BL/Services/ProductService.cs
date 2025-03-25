using BlazorApp.BL.Repositories;
using BlazorApp.Model.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BlazorApp.BL.Services
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> GetProduct(int id);
        Task UpdateProduct(ProductModel productModel);
        Task<ProductModel> CreateProduct(ProductModel productModel);
        Task<bool> ProductModelExists(int id);
        Task DeleteProduct(int id);
    }
    public class ProductService(IProductRepository productRepository, IDistributedCache cacheService) : IProductService
    {
        public async Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            var product = await productRepository.CreateProduct(productModel);
            await cacheService.RemoveAsync("list_products");
            return product;
        }

        public Task<ProductModel> GetProduct(int id)
        {
            return productRepository.GetProduct(id);
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var cacheValue = await cacheService.GetStringAsync("list_products");
            if (!string.IsNullOrEmpty(cacheValue))
            {
                return JsonConvert.DeserializeObject<List<ProductModel>>(cacheValue);
            }
            var products = await productRepository.GetProducts();
            await cacheService.SetStringAsync("list_products", JsonConvert.SerializeObject(products));
            return products;

        }

        public Task<bool> ProductModelExists(int id)
        {
            return productRepository.ProductModelExists(id);
        }

        public async Task UpdateProduct(ProductModel productModel)
        {
            await productRepository.UpdateProduct(productModel);
            await cacheService.RemoveAsync("list_products");
        }
        public async Task DeleteProduct(int id)
        {
            await productRepository.DeleteProduct(id);
            await cacheService.RemoveAsync("list_products");
        }
    }
}
