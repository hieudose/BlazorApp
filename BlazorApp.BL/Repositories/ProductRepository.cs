using BlazorApp.Database.Data;
using BlazorApp.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.BL.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> GetProduct(int id);
        Task UpdateProduct(ProductModel productModel);
        Task<ProductModel> CreateProduct(ProductModel productModel);
        Task<bool> ProductModelExists(int id);
        Task DeleteProduct(int id);
    }
    public class ProductRepository(AppDbContext dbContext) : IProductRepository
    {
        public Task<List<ProductModel>> GetProducts()
        {
            return dbContext.Products.ToListAsync();
        }

        public Task<ProductModel> GetProduct(int id)
        {
            return dbContext.Products.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            dbContext.Products.Add(productModel);
            await dbContext.SaveChangesAsync();
            return productModel;
        }
        public async Task UpdateProduct(ProductModel productModel)
        {
            dbContext.Entry(productModel).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
        public Task<bool> ProductModelExists(int id)
        {
            return dbContext.Products.AnyAsync(e => e.ID == id);
        }
        public async Task DeleteProduct(int id)
        {
            var product = dbContext.Products.FirstOrDefault(n => n.ID == id);
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
