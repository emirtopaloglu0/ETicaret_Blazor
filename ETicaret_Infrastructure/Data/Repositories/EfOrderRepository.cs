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
            var db = await _context.Orders.Include(x => x.DeliveryCompany).Where(x => x.UserId == userId).ToListAsync();
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
            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
