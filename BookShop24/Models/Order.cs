using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace BookShop24.Models
{
    public class Order
    {
        [HiddenInput(DisplayValue = false)]// ID покупки
        public int Id { get; set; }
        
        [Required]
        [Range(1, 50000, ErrorMessage = "Недопустимая сумма")]
        [Display(Name = "Общая стоимость")]
        public int TotalPrice { get; set; }

        public DateTime Date { get; set; } // дата покупки
        public IEnumerable<OrderItem> Items { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите фамилию")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите свое имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Способ доставки")]
        public string DeliveryMethod { get; set; }
        [Display(Name = "Номер пункта самовывоза")]
        [Range(0, 10000)]
        public int PickUpPointId { get; set; }
        public string Status { get; set; }
    }
}