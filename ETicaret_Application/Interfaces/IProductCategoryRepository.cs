using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory?> GetById(int id);
        Task AddAsync(ProductCategory productCategory);
        Task<List<ProductCategory>?> GetCategoriesAsync();
    }
}
