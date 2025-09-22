using ETicaret_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.DTOs.OrderDTOs
{
    public class GetOrderWithItemsDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
