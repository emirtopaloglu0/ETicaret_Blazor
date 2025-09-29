namespace ETicaret_UI.ViewModals
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }
}
