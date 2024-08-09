using BlazorApp.BL.Repositories;
using BlazorApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            return productRepository.CreateProduct(productModel);
        }

        public Task<ProductModel> GetProduct(int id)
        {
            return productRepository.GetProduct(id);
        }

        public Task<List<ProductModel>> GetProducts()
        {
            return productRepository.GetProducts();
        }

        public Task<bool> ProductModelExists(int id)
        {
            return productRepository.ProductModelExists(id);
        }

        public Task UpdateProduct(ProductModel productModel)
        {
            return productRepository.UpdateProduct(productModel);
        }
        public Task DeleteProduct(int id)
        {
            return productRepository.DeleteProduct(id);
        }
    }
}
