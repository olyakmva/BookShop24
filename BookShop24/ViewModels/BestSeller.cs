using BookShop24.Models;

namespace BookShop24.ViewModels
{
    public class BestSeller
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public Book Book { get; set; }
    }
}
