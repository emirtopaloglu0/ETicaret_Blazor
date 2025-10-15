using Azure.Core;
using ETicaret_Application.DTOs.CartDTOs;
using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfCartItemRepository : ICartItemRepository
    {
        private readonly Entities.ETicaretDbContext _context;
        private readonly IProductRepository _productRepository;

        public EfCartItemRepository(Entities.ETicaretDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<bool> AddAsync(CartItemDTO cartItemDTO)
        {
            try
            {
                CartItem cartItem = new CartItem
                {
                    ProductId = cartItemDTO.ProductId,
                    UserId = cartItemDTO.UserId,
                    Quantity = cartItemDTO.Quantity,
                };
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUserHaveCartItems(int userId)
        {
            try
            {
                var response = await _context.CartItems.FirstOrDefaultAsync(x => x.UserId == userId);
                if (response == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var response = await _context.CartItems.FindAsync(id);
                if (response == null) return false;
                _context.CartItems.Remove(response);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CartItemDTO>?> GetByCartItemsByUserId(int userId)
        {
            var respnose = await _context.CartItems.Where(x => x.UserId == userId).ToListAsync();

            List<CartItemDTO> cartItems = new List<CartItemDTO>();
            foreach (var item in respnose)
            {
                var productDTO = await _productRepository.GetByIdAsync(item.ProductId);

                cartItems.Add(new CartItemDTO
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    ProductId = item.ProductId,
                    Product = productDTO,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt,

                });
            }
            return cartItems;
        }

        public async Task<bool> UpdateCategory(int id, int quantity)
        {
            try
            {
                var response = await _context.CartItems.FindAsync(id);
                if (response == null) return false;
                response.Quantity = quantity;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ClearCart(int userId)
        {
            try
            {
                var response = await _context.CartItems.Where(x => x.UserId == userId).ToListAsync();
                if (response == null) return false;

                _context.CartItems.RemoveRange(response);
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
