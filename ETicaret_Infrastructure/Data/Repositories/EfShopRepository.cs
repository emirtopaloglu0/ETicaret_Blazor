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
    public class EfShopRepository : IShopRepository
    {

        private readonly Entities.ETicaretDbContext _context;
        public EfShopRepository(Entities.ETicaretDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Shop shop)
        {
            var entityShop = new Entities.Shop
            {
                Name = shop.Name,
                Description = shop.Description
            };
            _context.Shops.Add(entityShop);
            await _context.SaveChangesAsync();
        }

        public async Task AddShopUser(ShopUser shopUser)
        {
            var entityShopUser = new Entities.ShopUser
            {
                UserId = shopUser.Id,
                ShopId = shopUser.ShopId
            };
            _context.ShopUsers.Add(entityShopUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
        }

        public async Task<Shop> GetByIdShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);

            return new Shop
            {
                Name = shop.Name,
                Description = shop.Description
            };
        }

        public async Task<List<Shop>> GetShops()
        {
            var shops = await _context.Shops.ToListAsync();
            List<Shop> shopList = new List<Shop>();
            foreach (var item in shops)
            {
                shopList.Add(new Shop
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                });
            }
            return shopList;
        }

        public async Task UpdateAsync(int id, Shop shop)
        {
            var oldShop = await _context.Shops.FindAsync(id);
            oldShop.Name = shop.Name;
            oldShop.Description = shop.Description;
            await _context.SaveChangesAsync();
        }
    }
}
