namespace ETicaret_UI.ViewModals
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopDesc { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsDelete { get; set; }
        public List<string> OtherImages { get; set; } = new();

    }
}
