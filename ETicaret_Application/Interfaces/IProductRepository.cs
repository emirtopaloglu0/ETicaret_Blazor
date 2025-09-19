using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret_Core.Entities;


namespace ETicaret_Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task DecreaseStockAsync(int productId, int quantity);
    }
}
