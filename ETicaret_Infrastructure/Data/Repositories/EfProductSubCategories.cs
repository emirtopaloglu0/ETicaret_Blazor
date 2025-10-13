using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfProductSubCategories : IProductSubCategoryRepository
    {
        private readonly Entities.ETicaretDbContext _context;
        public EfProductSubCategories(Entities.ETicaretDbContext context) => _context = context;

        public async Task<bool> AddAsync(ProductSubCategory productSubCategory)
        {
            try
            {
                var dbSubCategory = new Entities.ProductSubCategory
                {
                    CategoryId = productSubCategory.CategoryId,
                    Name = productSubCategory.Name,
                    Description = productSubCategory.Description,
                };
                await _context.ProductSubCategories.AddAsync(dbSubCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> DeleteCategory(int id)
        {
            var response = await _context.ProductSubCategories.FindAsync(id);
            if (response == null)
            {
                return false;
            }
            _context.ProductSubCategories.Remove(response);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductSubCategory?> GetById(int id)
        {
            var response = await _context.ProductSubCategories.FindAsync(id);
            ProductSubCategory productSubCategory = new ProductSubCategory
            {
                Id = response.Id,
                CategoryId = response.CategoryId,
                Name = response.Name,
                Description = response.Description,
            };
            return productSubCategory;
        }

        public async Task<List<ProductSubCategory>?> GetSubCategoriesAsync()
        {
            var response = await _context.ProductSubCategories.Include(x => x.Category).OrderBy(x => x.CategoryId).ToListAsync();
            List<ProductSubCategory> result = new List<ProductSubCategory>();
            foreach (var item in response)
            {
                result.Add(new ProductSubCategory
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category.Name,
                    Name = item.Name,
                    Description = item.Description,
                });
            }
            return result;
        }

        public async Task<bool> UpdateCategory(int id, int categoryId, string name, string description)
        {
            try
            {
                var subCategory = await _context.ProductSubCategories.FindAsync(id);
                if (subCategory == null)
                {
                    return false;
                }
                subCategory.CategoryId = categoryId;
                subCategory.Name = name;
                subCategory.Description = description;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public async Task<List<ProductSubCategory>> GetByCategoryId(int categoryId)
        {
            var response = await _context.ProductSubCategories.Where(x => x.CategoryId == categoryId).ToListAsync();
            List<ProductSubCategory> result = new List<ProductSubCategory>();
            foreach (var item in response)
            {
                result.Add(new ProductSubCategory
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Name = item.Name,
                    Description = item.Description,
                });
            }
            return result;
        }
    }
}
