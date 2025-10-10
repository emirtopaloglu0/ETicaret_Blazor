using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfProductImageRepository : IProductImageRepository
    {
        private readonly ETicaretDbContext _context;
        public EfProductImageRepository(ETicaretDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(List<ProductImageDTO> productImageDTO)
        {
            try
            {
                List<ProductImage> productImage = new List<ProductImage>();
                foreach (var item in productImageDTO)
                {
                    productImage.Add(new ProductImage
                    {
                        ProductId = item.ProductId,
                        ImageUrl = item.ImageUrl,
                    });
                }

                await _context.ProductImages.AddRangeAsync(productImage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ProductImageDTO>> GetByProductId(int productId)
        {
            try
            {
                var response = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
                List<ProductImageDTO> productImageDTO = new List<ProductImageDTO>();
                foreach (var item in response)
                {
                    productImageDTO.Add(new ProductImageDTO
                    {
                        Id = item.ProductId,
                        ProductId = item.ProductId,
                        ImageUrl = item.ImageUrl,
                    });
                }

                return productImageDTO;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteAsync(int imageId)
        {
            try
            {
                var response = await _context.ProductImages.FindAsync(imageId);
                if (response == null)
                {
                    return false;
                }
                _context.ProductImages.Remove(response);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int productId, List<ProductImageDTO> productImages)
        {
            try
            {
                var oldProductImage = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
                if (oldProductImage == null)
                {
                    return false;
                }

                if (productImages.Count > oldProductImage.Count)
                {
                    try
                    {
                        await SpecialUpdateCondition(productId, productImages);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                int i = 0;
                List<ProductImage> updatedProductImages = new List<ProductImage>();
                foreach (var item in oldProductImage)
                {
                    if (i >= productImages.Count)
                    {
                        _context.ProductImages.Remove(item);
                    }
                    else
                    {
                        item.ImageUrl = productImages[i].ImageUrl;
                        updatedProductImages.Add(item);
                        i++;
                    }

                }
                _context.ProductImages.UpdateRange(updatedProductImages);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task SpecialUpdateCondition(int productId, List<ProductImageDTO> productImages)
        {
            var deletableProductImages = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            _context.ProductImages.RemoveRange(deletableProductImages);
            await _context.SaveChangesAsync();

            List<ProductImageDTO> productImageDTOs = new List<ProductImageDTO>();
            foreach (var item in productImages)
            {
                productImageDTOs.Add(new ProductImageDTO
                {
                    ProductId = productId,
                    ImageUrl = item.ImageUrl
                });
            }
            await AddAsync(productImageDTOs);
        }

        public async Task<bool> IsImageAlreadyExist(int productId)
        {
            try
            {
                var response = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (response == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
