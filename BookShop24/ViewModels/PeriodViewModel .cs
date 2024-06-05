using System.ComponentModel.DataAnnotations;

namespace BookShop24.ViewModels
{
    public class PeriodViewModel
    {
        [Required(ErrorMessage ="Пожалуйста, укажите дату")]
        [Display(Name ="Дата начала")]
        public DateTime Start { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите дату")]
        [Display(Name = "Дата конца")]
        public DateTime End { get; set; }
        public List<BestSeller> BestSellers { get; set; }
    }
}
