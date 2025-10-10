using ETicaret_Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Interfaces
{
    public interface IProductImageRepository
    {
        Task<bool> AddAsync(List<ProductImageDTO> productImageDTO);
        Task<List<ProductImageDTO>> GetByProductId(int productId);
        Task<bool> UpdateAsync(int productId, List<ProductImageDTO> productImages);
        Task<bool> DeleteAsync(int imageId);
        Task<bool> IsImageAlreadyExist(int productId);

    }
}
