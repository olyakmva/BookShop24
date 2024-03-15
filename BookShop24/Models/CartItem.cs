using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop24.Models
{
    [Table("Cart")]
    public class CartItem
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string CartId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Некорректное значение")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }
        public Book SelectBook { get; set; }
    }
}