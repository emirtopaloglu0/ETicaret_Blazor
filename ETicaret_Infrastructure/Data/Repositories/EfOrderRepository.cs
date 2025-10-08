using ETicaret_Application.Interfaces;
using ETicaret_Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret_Application.DTOs;
using System.Globalization;
using ETicaret_Application.DTOs.OrderDTOs;
using ETicaret_UI.Enums;


namespace ETicaret_Infrastructure.Data.Repositories
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly ETicaretDbContext _context;
        public EfOrderRepository(ETicaretDbContext context) => _context = context;

        public async Task AddAsync(ETicaret_Core.Entities.Order order)
        {
            var dbOrder = new Entities.Order
            {
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                DeliveryDate = order.DeliveryDate,
                DeliveryCompanyId = order.DeliveryCompanyId
            };

            dbOrder.OrderItems = order.Items.Select(i => new Entities.OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            _context.Orders.Add(dbOrder);
            await _context.SaveChangesAsync();
            order.Id = dbOrder.Id;
        }

        public async Task<ETicaret_Core.Entities.Order?> GetByIdAsync(int id)
        {
            var db = await _context.Orders
                       .Include(o => o.OrderItems)
                       .FirstOrDefaultAsync(o => o.Id == id);
            if (db == null) return null;
            var domain = new ETicaret_Core.Entities.Order
            {
                Id = db.Id,
                OrderDate = db.OrderDate,
                TotalAmount = db.TotalAmount,
                Status = db.Status
            };

            return domain;
        }

        public async Task<List<ETicaret_Core.Entities.Order>?> GetOrdersAsync(int userId)
        {
            var db = await _context.Orders.Include(x => x.DeliveryCompany).Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
            List<ETicaret_Core.Entities.Order> dtoList = new List<ETicaret_Core.Entities.Order>();
            foreach (var dbOrder in db)
            {
                dtoList.Add(new ETicaret_Core.Entities.Order
                {
                    Id = dbOrder.Id,
                    OrderDate = dbOrder.OrderDate,
                    TotalAmount = dbOrder.TotalAmount,
                    Status = dbOrder.Status,
                    ShippingAddress = dbOrder.ShippingAddress,
                    DeliveryDate = dbOrder.DeliveryDate,
                    DeliveryCompanyId = dbOrder.DeliveryCompanyId,
                    DeliveryCompanyName = dbOrder.DeliveryCompany.Name
                });
            }
            return dtoList;
        }

        public async Task<ETicaret_Core.Entities.Order?> GetWithItemsAsync(int id)
        {
            var dbOrder = await _context.Orders.Include(x => x.OrderItems)
                .Include(x => x.DeliveryCompany)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbOrder == null) return null;
            List<ETicaret_Core.Entities.OrderItem> orderItemsList = new List<ETicaret_Core.Entities.OrderItem>();
            foreach (var item in dbOrder.OrderItems)
            {
                var orderItem = new ETicaret_Core.Entities.OrderItem(item.ProductId, item.Quantity, item.UnitPrice)
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                };
                orderItemsList.Add(orderItem);
            }
            var domain = new ETicaret_Core.Entities.Order
            {
                Id = dbOrder.Id,
                OrderDate = dbOrder.OrderDate,
                DeliveryDate = dbOrder.DeliveryDate,
                TotalAmount = dbOrder.TotalAmount,
                Status = dbOrder.Status,
                ShippingAddress = dbOrder.ShippingAddress,
                DeliveryCompanyId = dbOrder.DeliveryCompanyId,
                DeliveryCompanyName = dbOrder.DeliveryCompany.Name,
                orderItems = orderItemsList
            };
            return domain;
        }
        public async Task UpdateCargoStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");

            if (status == OrderStatus.Kargoda)
            {
                order.DeliveryDate = DateTime.Now.AddDays(3);
            }
            if (status == OrderStatus.Tamamlandi)
            {
                order.DeliveryDate = DateTime.Now;
            }
            if (status == OrderStatus.Iptal || status == OrderStatus.IadeOnaylandı)
            {
                var _orderItems = await _context.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync();
                foreach (var item in _orderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    product.Stock += item.Quantity;
                    await _context.SaveChangesAsync();
                }
            }

            order.Status = status;
            await _context.SaveChangesAsync();
        }
        public async Task<List<GetOrderDto>> GetByCompanyId(int id)
        {
            var orders = await _context.Orders.Include(x => x.DeliveryCompany).Where(x => x.DeliveryCompanyId == id)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
            List<GetOrderDto> result = new List<GetOrderDto>();
            foreach (var order in orders)
            {
                result.Add(new GetOrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    DeliveryDate = order.DeliveryDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    CompanyName = order.DeliveryCompany.Name,
                    CompanyId = order.DeliveryCompanyId
                });
            }
            return result;
        }
        public async Task<List<GetOrderWithItemsDto>> GetRefundRequestOrders(int shopId)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.Shop)
                .Include(x => x.DeliveryCompany)
                .Where(x => x.Status == OrderStatus.IadeTalepEdildi).ToListAsync();

            List<GetOrderWithItemsDto> result = new List<GetOrderWithItemsDto>();
            foreach (var order in orders)
            {
                List<OrderItemDTO> orderItemDTOs = order.OrderItems
                    .Select(x => new OrderItemDTO(x.ProductId, x.Quantity, x.UnitPrice)
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        ProductName = x.Product.Name,
                        ProductURL = x.Product.ImageUrl
                    })
                    .ToList();


                foreach (var item in order.OrderItems)
                {
                    if (item.Product.ShopId == shopId && !result.Any(x => x.Id == order.Id))
                    {
                        result.Add(new GetOrderWithItemsDto
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate,
                            ShippingAddress = order.ShippingAddress,
                            TotalAmount = order.TotalAmount,
                            Status = order.Status,
                            CompanyName = order.DeliveryCompany.Name,
                            CompanyId = order.DeliveryCompanyId,
                            Items = orderItemDTOs
                        });
                        break;
                    }
                }
            }
            return result;
        }

        public async Task<List<GetOrderWithItemsDto>> GetWithItemsByShopId(int shopId)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).ThenInclude(x => x.Category)
                .Include(x => x.DeliveryCompany).ToListAsync();

            List<GetOrderWithItemsDto> result = new List<GetOrderWithItemsDto>();
            foreach (var order in orders)
            {
                List<OrderItemDTO> orderItemDTOs = order.OrderItems
                    .Select(x => new OrderItemDTO(x.ProductId, x.Quantity, x.UnitPrice)
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        ProductName = x.Product.Name,
                        ProductCategory = x.Product.Category.Name,
                        ProductURL = x.Product.ImageUrl
                    })
                    .ToList();


                foreach (var item in order.OrderItems)
                {
                    if (item.Product.ShopId == shopId && !result.Any(x => x.Id == order.Id))
                    {
                        result.Add(new GetOrderWithItemsDto
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate,
                            ShippingAddress = order.ShippingAddress,
                            TotalAmount = order.TotalAmount,
                            Status = order.Status,
                            CompanyName = order.DeliveryCompany.Name,
                            CompanyId = order.DeliveryCompanyId,
                            Items = orderItemDTOs
                        });
                        break;
                    }
                }
            }
            return result;
        }

    }
}
