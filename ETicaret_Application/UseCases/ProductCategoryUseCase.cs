using ETicaret_Application.DTOs.ProductDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.UseCases
{
    public class ProductCategoryUseCase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private IProductCategoryRepository _productCategoryRepository;
        private readonly ICurrentUserService _currentUser;

        public ProductCategoryUseCase(IOrderRepository orderRepo, IProductRepository productRepo,
                              ICurrentUserService currentUser, IProductCategoryRepository productCategoryRepository)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _currentUser = currentUser;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<int> ExecuteCreate(CreateProductCategoryDto dto)
        {
            //if (_currentUser.UserId == null) throw new UnauthorizedAccessException();

            var productCategory = new ETicaret_Core.Entities.ProductCategory
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            await _productCategoryRepository.AddAsync(productCategory);
            return productCategory.Id;
        }

        public async Task<List<GetProductCategoryDto>> ExecuteListAsync()
        {
            var response = await _productCategoryRepository.GetCategoriesAsync();
            List<GetProductCategoryDto> categoryList = new List<GetProductCategoryDto>();
            foreach (var item in response)
            {
                categoryList.Add(new GetProductCategoryDto
                {
                    Name = item.Name,
                    Description = item.Description
                });
            }
            return categoryList;
        }
        public async Task<GetProductCategoryDto?> ExecuteGetById(int id)
        {
            var response = await _productCategoryRepository.GetById(id);
            var domain = new GetProductCategoryDto
            {
                Name = response.Name,
                Description = response.Description,
            };
            return domain;
        }
    }
}
