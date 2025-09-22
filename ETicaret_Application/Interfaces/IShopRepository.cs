using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IShopRepository
    {
        Task AddAsync(Shop shop);
        Task<List<Shop>> GetShops();
        Task<Shop> GetByIdShop(int id);
        Task UpdateAsync(int id, Shop shop);
        Task DeleteAsync(int id);
    }
}
