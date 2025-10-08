using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public OrderItemDTO(int productId, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }

        public string ProductURL { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
