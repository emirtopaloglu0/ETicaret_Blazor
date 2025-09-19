using ETicaret_Application.DTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.Services;
using ETicaret_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ETicaret_Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICurrentUserService _currentUser;


        public CreateOrderUseCase(IOrderRepository orderRepo, IProductRepository productRepo,
                              ICurrentUserService currentUser)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _currentUser = currentUser;
        }

        public async Task<int> ExecuteAsync(CreateOrderDto dto)
        {
            //if (_currentUser.UserId == null) throw new UnauthorizedAccessException();

            var order = new ETicaret_Core.Entities.Order
            {
                UserId = /*_currentUser.UserId.Value*/ 1,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = dto.ShippingAddress,
                DeliveryCompanyId = dto.DelivererCompanyId
            };

            foreach (var it in dto.Items)
            {
                var product = await _productRepo.GetByIdAsync(it.ProductId)
                              ?? throw new Exception($"Product {it.ProductId} not found");
                if (product.Stock < it.Quantity) throw new Exception("Not enough stock");

                order.AddItem(it.ProductId, it.Quantity, product.Price);
                await _productRepo.DecreaseStockAsync(it.ProductId, it.Quantity);
            }

            await _orderRepo.AddAsync(order);

            return order.Id;
        }

    }
}
