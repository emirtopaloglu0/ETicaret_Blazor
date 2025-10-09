using ETicaret_Application.DTOs.OrderDTOs;
using ETicaret_Application.Interfaces;
using ETicaret_Application.Services;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.UseCases
{
    public class GetOrderUseCase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICurrentUserService _currentUser;


        public GetOrderUseCase(IOrderRepository orderRepo, IProductRepository productRepo,
                              ICurrentUserService currentUser)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _currentUser = currentUser;
        }

        public async Task<GetOrderDto?> ExecuteByIdAsync(int id)
        {
            //if (_currentUser.UserId == null) throw new UnauthorizedAccessException();

            var response = await _orderRepo.GetByIdAsync(id);
            var test = new GetOrderDto
            {
                Id = response.Id,
                OrderDate = response.OrderDate,
                TotalAmount = response.TotalAmount,
                Status = response.Status
            };
            return test;
        }

        public async Task<List<GetOrderDto>> ExecuteListAsync(int userId)
        {
            //if (_currentUser.UserId == null) throw new UnauthorizedAccessException();

            var response = await _orderRepo.GetOrdersAsync(userId);
            List<GetOrderDto> getOrderDtos = new List<GetOrderDto>();
            foreach (var item in response)
            {
                getOrderDtos.Add(new GetOrderDto
                {
                    Id = item.Id,
                    OrderDate = item.OrderDate,
                    DeliveryDate = item.DeliveryDate,
                    ShippingAddress = item.ShippingAddress,
                    TotalAmount = item.TotalAmount,
                    Status = item.Status,
                    CompanyName = item.DeliveryCompanyName,
                    CompanyId = item.DeliveryCompanyId,
                });
            }
            return getOrderDtos;
        }

        public async Task<GetOrderWithItemsDto> ExecuteWithItemsAsync(int id)
        {
            //if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
            var response = await _orderRepo.GetWithItemsAsync(id);
            List<OrderItemDTO> orderItemList = new List<OrderItemDTO>();

            foreach (var item in response.orderItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                var orderItem = new OrderItemDTO(item.ProductId, item.Quantity, item.UnitPrice)
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductName = product.Name,
                    ProductURL = product.ImageUrl
                };
                orderItemList.Add(orderItem);
            }

            //if (_currentUser.UserId != response.UserId) throw new UnauthorizedAccessException();
            var getOrderWithItem = new GetOrderWithItemsDto
            {
                OrderDate = response.OrderDate,
                DeliveryDate = response.DeliveryDate,
                ShippingAddress = response.ShippingAddress,
                TotalAmount = response.TotalAmount,
                Status = response.Status,
                CompanyName = response.DeliveryCompanyName,
                CompanyId = response.DeliveryCompanyId,
                Items = orderItemList
            };

            return getOrderWithItem;


        }
    }
}
