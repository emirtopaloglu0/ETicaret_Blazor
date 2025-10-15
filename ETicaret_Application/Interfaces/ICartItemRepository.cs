using ETicaret_Application.DTOs.CartDTOs;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface ICartItemRepository
    {
        Task<List<CartItemDTO>?> GetByCartItemsByUserId(int userId);
        Task<bool> AddAsync(CartItemDTO cartItem);
        Task<bool> UpdateCategory(int id, int quantity);
        Task<bool> DeleteCategory(int id);
        Task<bool> IsUserHaveCartItems(int userId);
        Task<bool> ClearCart(int userId);
    }
}
