namespace RiseCup.Domain.Models
{
    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    
        public string ProductId { get; set; }
        public Product Product { get; set; }
    
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}