using System.Data.Entity;
using RiseCup.Domain.Models;

namespace RiseCup.Database
{
    public class RiseCupContext : DbContext
    {
        public RiseCupContext() : base("name=MSSQL")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}