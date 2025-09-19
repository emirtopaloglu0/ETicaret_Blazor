using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Core.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public DateTime? DeliveryDate { get; set; }

        public int DeliveryCompanyId { get; set; }
        public string DeliveryCompanyName { get; set; }


        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public void AddItem(int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0");
            _items.Add(new OrderItem(productId, quantity, unitPrice));
            RecalculateTotal();
        }

        private void RecalculateTotal() => TotalAmount = _items.Sum(i => i.Quantity * i.UnitPrice);

        public void SetStatus(string status) => Status = status;
    }
}
