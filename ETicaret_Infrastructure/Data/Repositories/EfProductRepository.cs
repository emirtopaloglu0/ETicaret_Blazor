using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Core.Entities;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ETicaretDbContext _context;

        public EfProductRepository(ETicaretDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ETicaret_Core.Entities.Product product)
        {
            try
            {
                Entities.Product productEntity = new Entities.Product
                {
                    CategoryId = product.CategoryId,
                    SubCategoryId = product.SubCategoryId,
                    ShopId = product.ShopId,
                    Name = product.Name,
                    Description = product.Description,
                    Stock = product.Stock,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                };
                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
                return productEntity.Id;
            }
            catch
            {
                return 0;
            }
        }

        public async Task DecreaseStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {productId}");

            if (product.Stock < quantity)
                throw new InvalidOperationException($"Yeterli stok yok. Mevcut: {product.Stock}, İstenen: {quantity}");

            product.Stock -= quantity;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                product.IsDelete = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ProductDTO>?> GetAllProduct()
        {
            var allProducts = await _context.Products.Where(x => !x.IsDelete)
                .Include(x => x.Category)
                .Include(x => x.Shop)
                .ToListAsync();
            List<ProductDTO> products = new List<ProductDTO>();
            foreach (var product in allProducts)
            {
                products.Add(new ProductDTO
                {
                    Id = product.Id,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    CategoryDesc = product.Category.Description,
                    SubCategoryId = product.SubCategoryId,
                    SubCategoryName = product.SubCategory?.Name,
                    ShopId = product.ShopId,
                    ShopName = product.Shop.Name,
                    ShopDesc = product.Shop.Description,
                    Name = product.Name,
                    Description = product.Description,
                    Stock = product.Stock,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                });
            }
            return products;
        }

        public async Task<List<ProductDTO>?> GetByCategoryIdAsync(int categoryId)
        {
            var productsByCategory = await _context.Products
                .Where(x => x.CategoryId == categoryId && !x.IsDelete)
                .Include(x => x.Category)
                .Include(x => x.Shop)
                .ToListAsync();
            List<ProductDTO> products = new List<ProductDTO>();
            foreach (var productByCategory in productsByCategory)
            {
                products.Add(new ProductDTO
                {
                    Id = productByCategory.Id,
                    CategoryId = productByCategory.CategoryId,
                    CategoryName = productByCategory.Category.Name,
                    CategoryDesc = productByCategory.Category.Description,
                    SubCategoryId = productByCategory.SubCategoryId,
                    SubCategoryName = productByCategory.SubCategory?.Name,
                    ShopId = productByCategory.ShopId,
                    ShopName = productByCategory.Shop.Name,
                    ShopDesc = productByCategory.Shop.Description,
                    Name = productByCategory.Name,
                    Description = productByCategory.Description,
                    Stock = productByCategory.Stock,
                    Price = productByCategory.Price,
                    ImageUrl = productByCategory.ImageUrl,
                    IsDelete = productByCategory.IsDelete,
                });
            }
            return products;
        }
        public async Task<List<ProductDTO>?> GetByShopIdAsync(int shopId)
        {
            var productsByShop = await _context.Products
                .Where(x => x.ShopId == shopId && !x.IsDelete)
                .Include(x => x.Category)
                .Include(x => x.Shop)
                .Include(x => x.SubCategory)
                .ToListAsync();
            List<ProductDTO> products = new List<ProductDTO>();
            foreach (var productByShop in productsByShop)
            {
                products.Add(new ProductDTO
                {
                    Id = productByShop.Id,
                    CategoryId = productByShop.CategoryId,
                    CategoryName = productByShop.Category.Name,
                    CategoryDesc = productByShop.Category.Description,
                    SubCategoryId = productByShop.SubCategoryId,
                    SubCategoryName = productByShop.SubCategory?.Name,
                    ShopId = productByShop.ShopId,
                    ShopName = productByShop.Shop.Name,
                    ShopDesc = productByShop.Shop.Description,
                    Name = productByShop.Name,
                    Description = productByShop.Description,
                    Stock = productByShop.Stock,
                    Price = productByShop.Price,
                    ImageUrl = productByShop.ImageUrl,
                    IsDelete = productByShop.IsDelete,
                });
            }
            return products;
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            var response = await _context.Products.Include(x => x.Category).Include(x => x.SubCategory)
                .Include(x => x.Shop)
                .FirstOrDefaultAsync(p => p.Id == id);
            var domain = new ProductDTO
            {
                Id = response.Id,
                CategoryId = response.CategoryId,
                CategoryName = response.Category.Name,
                CategoryDesc = response.Category.Description,
                SubCategoryId = response.SubCategoryId,
                SubCategoryName = response.SubCategory?.Name,
                ShopId = response.ShopId,
                ShopName = response.Shop.Name,
                ShopDesc = response.Shop.Description,
                Name = response.Name,
                Description = response.Description,
                Stock = response.Stock,
                Price = response.Price,
                ImageUrl = response.ImageUrl,
            };
            return domain;
        }
        public async Task<ProductDTO?> GetByIdWithCompanyAndShopsAsync(int productId)
        {
            var response = await _context.Products.Include(x => x.Category).Include(x => x.Shop).FirstOrDefaultAsync(p => p.Id == productId);
            var domain = new ProductDTO
            {
                Id = response.Id,
                CategoryId = response.CategoryId,
                CategoryName = response.Category.Name,
                CategoryDesc = response.Category.Description,
                SubCategoryId = response.SubCategoryId,
                SubCategoryName = response.SubCategory?.Name,
                ShopId = response.ShopId,
                ShopName = response.Shop.Name,
                ShopDesc = response.Shop.Description,
                Name = response.Name,
                Description = response.Description,
                Stock = response.Stock,
                Price = response.Price,
                ImageUrl = response.ImageUrl,
            };
            return domain;
        }

        public async Task<bool> UpdateAsync(int id, ETicaret_Core.Entities.Product product, int shopUserId = 0)
        {
            try
            {
                var oldProduct = await _context.Products.FindAsync(id);
                if (shopUserId != 0)
                {
                    var shop = await _context.ShopUsers.FirstOrDefaultAsync(x => x.UserId == shopUserId);
                    if (shop.ShopId != oldProduct.ShopId)
                    {
                        return false;
                    }
                    oldProduct.ShopId = shop.ShopId;
                }
                else
                {
                    oldProduct.ShopId = product.ShopId;
                }
                oldProduct.CategoryId = product.CategoryId;
                oldProduct.Name = product.Name;
                oldProduct.Description = product.Description;
                oldProduct.SubCategoryId = product.SubCategoryId;
                oldProduct.Stock = product.Stock;
                oldProduct.Price = product.Price;
                oldProduct.ImageUrl = product.ImageUrl;
                oldProduct.IsDelete = product.IsDelete;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
