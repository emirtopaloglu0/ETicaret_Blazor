using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public int ShopId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsDelete { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
