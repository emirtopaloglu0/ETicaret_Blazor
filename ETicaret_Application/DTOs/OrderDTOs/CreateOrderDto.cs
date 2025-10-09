using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; } = "";
        public int DelivererCompanyId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
