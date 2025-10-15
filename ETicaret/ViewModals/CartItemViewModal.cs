namespace ETicaret_UI.ViewModals
{
    public class CartItemViewModal
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }
        public ProductViewModel? Product { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
