using BookShop24.Models;
using BookShop24.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop24.Controllers
{
    [Authorize(Roles ="manager")]
    public class ManagerController : Controller
    {
        BookContext db;
        const int BestSellerSalesNumber = 5;
        DateTime InitialDate = new DateTime(2024, 04, 18);
        public ManagerController(BookContext bookContext) 
        { 
            db= bookContext;
        }

        public IActionResult GetBestsellerList()
        {
            DateTime start = DateTime.Now.AddDays(-30);
            DateTime end = DateTime.Now;

            PeriodViewModel periodViewModel = GetBooks(start, end);

            return View(periodViewModel);
        }

        private PeriodViewModel GetBooks(DateTime start, DateTime end)
        {
            var orders = db.Orders.Where(o => o.Date > start && o.Date < end).ToList();
            var bookSalesNumberDictionary = new Dictionary<int, int>();

            foreach (var order in orders)
            {
                var orderItems = db.Items.Where(i => i.OrderId == order.Id).ToList();
                foreach (var item in orderItems)
                {
                    if (!bookSalesNumberDictionary.ContainsKey(item.BookId))
                    {
                        bookSalesNumberDictionary.Add(item.BookId, 0);
                    }
                    bookSalesNumberDictionary[item.BookId]++;
                }
            }
            var list = (from pair in bookSalesNumberDictionary
                        where pair.Value > BestSellerSalesNumber
                        select new BestSeller()
                        {
                            BookId = pair.Key,
                            Quantity = pair.Value
                        }).ToList();
            foreach (var item in list)
            {
                var book = db.Books.Find(item.BookId);
                if (book != null)
                    item.Book = book;
            }

            PeriodViewModel periodViewModel = new PeriodViewModel()
            {
                Start = start,
                End = end,
                BestSellers = list
            };
            return periodViewModel;
        }

        [HttpPost]
        public IActionResult GetBestsellerList(PeriodViewModel model)
        {
            ViewBag.Msg = "Бестселлеры";
            if(model.Start >  model.End)
            {
                ViewBag.Msg = "Вы неверно задали диапазон дат";
            }
            if (model.End < InitialDate) 
            {
                ViewBag.Msg = "Вы неверно задали диапазон дат: наш магазин работает с 18 апреля 2024";
            }
            return View(GetBooks(model.Start,model.End));

        }
    }
}
