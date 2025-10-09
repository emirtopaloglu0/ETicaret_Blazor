namespace ETicaret_UI.ViewModals
{
    public class GetOrderWithItemsViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
