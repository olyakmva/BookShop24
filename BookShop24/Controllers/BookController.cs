using BookShop24.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BookShop24.Controllers
{
    public class BookController : Controller
    {
        BookContext _db;
        IWebHostEnvironment _environment;
        const int ImageWidth = 150;
        const int ImageHeight = 200; 
        public BookController(BookContext context, IWebHostEnvironment hostEnvironment)
        {
            _db = context;
            _environment = hostEnvironment;
            //var zanr1 = new Category() { Name = "Учебники" };
            //var zanr2 = new Category() { Name = "Фэнтези" };
            //var book1 = new Book
            //{
            //    Author = "Троелсен Э.",
            //    Name = "Язык программирования C#",
            //    Number = 12,
            //    Year = 2020,
            //    Price = 4230,
            //    Category = zanr1
            //};
            //var book2 = new Book
            //{
            //    Author = "Скиена С.",
            //    Name = "Наука о данных",
            //    Number = 22,
            //    Year = 2021,
            //    Price = 1230,
            //    Category = zanr1
            //};
            //var book3 = new Book
            //{
            //    Author = "Демидович Б.Н.",
            //    Name = "Сборник задач и упражнений по математическому анализу",
            //    Number = 55,
            //    Year = 2020,
            //    Price = 975,
            //    Category = zanr1
            //};
            //var book4 = new Book
            //{
            //    Author = "Фрай Макс",
            //    Name = "Чужак",
            //    Number = 27,
            //    Year = 2019,
            //    Price = 450,
            //    Category = zanr2
            //};
            //var book5 = new Book
            //{
            //    Author = "Громыко О.",
            //    Name = "Верные враги",
            //    Number = 12,
            //    Year = 2020,
            //    Price = 520,
            //    Category = zanr2
            //};
            //var book6 = new Book
            //{
            //    Author = "Лукьяненко С.",
            //    Name = "Спектр",
            //    Number = 33,
            //    Year = 2018,
            //    Price = 330,
            //    Category = zanr2
            //};
            ////_db.Categories.Add(zanr1);
            ////_db.Categories.Add(zanr2);
            //_db.Books.AddRange(new[] { book1, book2,book4,book5,book6, book3 });
            //_db.SaveChanges();

        }
        public IActionResult Index(SortOrder sortOrder = SortOrder.AuthorAsc)
        {
            var books = _db.Books.Include(b => b.Category).ToList();
            switch (sortOrder)
            {
                case SortOrder.AuthorAsc:
                    books = books.OrderBy(b => b.Author).ToList();
                    break;
                case SortOrder.AuthorDesc:
                    books = books.OrderByDescending(b => b.Author).ToList();
                    break;
                case SortOrder.NameAsc: books = books.OrderBy(b => b.Name).ToList(); break;
                case SortOrder.NameDesc: books = books.OrderByDescending(b => b.Name).ToList(); break;
                case SortOrder.NumberAsc: books = books.OrderBy(b => b.Number).ToList(); break;
                case SortOrder.NumberDesc: books = books.OrderByDescending(b => b.Number).ToList(); break;
                case SortOrder.PriceAsc: books = books.OrderBy(b => b.Price).ToList(); break;
                case SortOrder.PriceDesc: books = books.OrderByDescending(b => b.Price).ToList(); break;
                case SortOrder.YearAsc: books = books.OrderBy(b => b.Year).ToList(); break;
                case SortOrder.YearDesc: books = books.OrderByDescending(b => b.Year).ToList(); break;
                case SortOrder.CategoryAsc: books = books.OrderBy(b => b.Category.Name).ToList(); break;
                case SortOrder.CategoryDesc: books = books.OrderByDescending(b => b.Category.Name).ToList(); break;
            }
            ViewData["AuthorSort"] = sortOrder == SortOrder.AuthorAsc ? SortOrder.AuthorDesc : SortOrder.AuthorAsc;
            ViewData["NameSort"] = sortOrder == SortOrder.NameAsc ? SortOrder.NameDesc : SortOrder.NameAsc;
            ViewData["PriceSort"] = sortOrder == SortOrder.PriceAsc ? SortOrder.PriceDesc : SortOrder.PriceAsc;
            ViewData["CategorySort"] = sortOrder == SortOrder.CategoryAsc ? SortOrder.CategoryDesc : SortOrder.CategoryAsc;
            ViewData["NumberSort"] = sortOrder == SortOrder.NumberAsc ? SortOrder.NumberDesc : SortOrder.NumberAsc;
            ViewData["YearSort"] = sortOrder == SortOrder.YearAsc ? SortOrder.YearDesc : SortOrder.YearAsc;
            return View(books);
        }
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(_db.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book, IFormFile upload)
        {
            if (upload != null)
            { 
                string fileName =Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.LastIndexOf('.'));
                if(extFile.Contains("png")|| extFile.Contains("bmp")|| extFile.Contains("jpg")
                    || extFile.Contains("jpeg"))
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x=> x.Resize(ImageWidth, ImageHeight));
                    string path = "\\wwwroot\\images\\" + fileName;
                    var hostPath = _environment.ContentRootPath + path;
                    image.Save(hostPath);
                    book.ImageUrl = fileName;
                }         
            }
            _db.Books.Add(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var book = _db.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();
            return View(book);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var book = _db.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();
            ViewBag.Category = new SelectList(_db.Categories, "Id", "Name");
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit(Book book, IFormFile upload)
        {
            if (upload != null)
            {
                string fileName = Path.GetFileName(upload.FileName);
                var extFile = fileName.Substring(fileName.LastIndexOf('.'));
                if (extFile.Contains("png") || extFile.Contains("bmp") || extFile.Contains("jpg")
                    || extFile.Contains("jpeg"))
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x => x.Resize(ImageWidth, ImageHeight));
                    string path = "\\wwwroot\\images\\" + fileName;
                    var hostPath = _environment.ContentRootPath + path;
                    image.Save(hostPath);
                    book.ImageUrl = fileName;
                }
            }

            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var book = _db.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();
            return View(book);
        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            if(book!=null)
            {
                _db.Entry(book).State = EntityState.Deleted;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
