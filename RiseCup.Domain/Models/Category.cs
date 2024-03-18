using System.Collections.Generic;

namespace RiseCup.Domain.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}