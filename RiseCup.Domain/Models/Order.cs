using System;
using System.Collections.Generic;

namespace RiseCup.Domain.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public decimal TotalAmount { get; set; } 
        public string Status { get; set; } 
    }
}