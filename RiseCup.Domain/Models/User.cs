using System.Collections.Generic;

namespace RiseCup.Domain.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        
        public ICollection<Order> Orders { get; set; }
    }
}