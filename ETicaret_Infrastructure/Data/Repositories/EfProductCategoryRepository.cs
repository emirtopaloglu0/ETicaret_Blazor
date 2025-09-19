using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using ETicaret_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfProductCategoryRepository : IProductCategoryRepository
    {
        private readonly Entities.ETicaretDbContext _context;
        public EfProductCategoryRepository(Entities.ETicaretDbContext context) => _context = context;
        public async Task AddAsync(ProductCategory productCategory)
        {
            var dbProductCategory = new Entities.ProductCategory
            {
                Name = productCategory.Name,
                Description = productCategory.Description,
            };
            _context.ProductCategories.Add(dbProductCategory);
            await _context.SaveChangesAsync();
            productCategory.Id = dbProductCategory.Id;

        }

        public async Task<ProductCategory?> GetById(int id)
        {
            var dbCategory = await _context.ProductCategories.FindAsync(id);
            if (dbCategory == null) return null;
            var domain = new ProductCategory
            {
                Name = dbCategory.Name,
                Description = dbCategory.Description,
            };
            return domain;
        }
        public async Task<List<ProductCategory>> GetCategoriesAsync()
        {
            var dbCategories = await _context.ProductCategories.ToListAsync();
            List<ProductCategory> productCategories = new List<ProductCategory>();
            foreach (var item in dbCategories)
            {
                productCategories.Add(new ProductCategory
                {
                    Name = item.Name,
                    Description = item.Description
                });
            }

            return productCategories;
        }
    }
}
