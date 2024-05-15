using BookShop24.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookShop24.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        BookContext db;
        public HomeController(ILogger<HomeController> logger, BookContext context)
        {
            _logger = logger;
            db= context;
        }

        public IActionResult Index()
        {
            var books = db.Books.Include(b=> b.Category).ToList();
            ViewBag.Msg = "Наши книги";
            return View(books);
        }
        public IActionResult Search(string searchStr)
        {
            var books = db.Books.Include(b => b.Category).ToList();
            if (string.IsNullOrEmpty(searchStr))
            {
                ViewBag.Msg = "Напишите в строке поиска название или автора книги, что Вы ищете";
                return View("Index",books);
            }
            
                ///TODO обработать строку поиска
              
                var list = books.FindAll(b =>
                        b.Name.Contains(searchStr) || b.Author.Contains(searchStr) ||
                        b.Year.ToString().Contains(searchStr) || b.Category.Name.Contains(searchStr));
                if (list.Count == 0)
                {
                    ViewBag.Msg = "К сожалению, мы ничего не нашли по Вашему запросу";
                    return View("Index",books);
                }
                else ViewBag.Msg = "Вот, что мы нашли по Вашему запросу:";
                return View("Index", list);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}