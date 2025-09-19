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

        public async Task<ETicaret_Core.Entities.Product?> GetByIdAsync(int id)
        {
            var test = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            var test2 = new ETicaret_Core.Entities.Product
            {
                Id = test.Id,
                CategoryId = test.CategoryId,
                ShopId = test.ShopId,
                Name = test.Name,
                Description = test.Description,
                Stock = test.Stock,
                Price = test.Price,
            };
            return test2;
        }
    }
}
