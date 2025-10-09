using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopDesc { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsDelete { get; set; }

    }
}
