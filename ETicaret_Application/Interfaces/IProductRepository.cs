using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Core.Entities;


namespace ETicaret_Application.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> AddAsync(Product product);
        Task<List<ProductDTO>> GetAllProduct();
        Task<Product?> GetByIdAsync(int id);
        Task<List<ProductDTO>?> GetByCategoryIdAsync(int categoryId);
        Task<List<ProductDTO>?> GetByShopIdAsync(int shopId);
        Task DecreaseStockAsync(int productId, int quantity);
        Task<bool> UpdateAsync(int id, Product product, int shopUserId = 0);
        Task<bool> DeleteAsync(int id);
        Task<ProductDTO> GetByIdWithCompanyAndShopsAsync(int productId);
    }
}
