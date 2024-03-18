namespace RiseCup.Domain.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }

}