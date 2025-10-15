using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IProductSubCategoryRepository
    {
        Task<ProductSubCategory?> GetById(int id);
        Task<bool> AddAsync(ProductSubCategory productSubCategory);
        Task<List<ProductSubCategory>?> GetSubCategoriesAsync();
        Task<bool> UpdateCategory(int id, int categoryId, string name, string description);
        Task<bool> DeleteCategory(int id);
        Task<List<ProductSubCategory>> GetByCategoryId(int categoryId);
    }
}
