
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmWeb.Models;

namespace SmWeb.Services
{
    public interface IProductDS
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(string ProductId);
        Task<bool> AddProduct(Product Product);
        Task UpdateProduct(Product Product);
        Task DeleteProduct(string ProductId);
    }
}
