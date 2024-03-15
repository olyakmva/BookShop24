using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookShop24.Models
{
    public class OrderItem
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Недопустимое количество")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        public Book TheBook { get; set; }
    }
}
