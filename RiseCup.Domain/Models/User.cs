using System.Collections.Generic;

namespace RiseCup.Domain.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

}