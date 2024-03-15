using Microsoft.EntityFrameworkCore;

namespace BookShop24.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<CartItem> ShoppingCarts { get; set; }
      
        public BookContext(DbContextOptions<BookContext> options)
             : base(options)
        {          
        }

    }
}