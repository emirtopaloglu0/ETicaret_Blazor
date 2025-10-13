namespace ETicaret_UI.ViewModals
{
    public class ProductSubCategoryViewModal
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
