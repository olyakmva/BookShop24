using BookShop24.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop24.Controllers
{
    public class CartController : Controller
    {
        BookContext db;
        public CartController(BookContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            string cartId=null;
            if (HttpContext.Request.Cookies.Keys.Count > 0 &&
               HttpContext.Request.Cookies.ContainsKey("cartId"))
            {
                cartId = HttpContext.Request.Cookies["cartId"];
            }
            List<CartItem> cartList = new List<CartItem>();
            if(cartId != null) 
            { 
                cartList = db.ShoppingCarts.Where(c=> c.CartId == cartId).ToList();
                int sum = 0;
                foreach (var cart in cartList) 
                {
                    var book = db.Books.Find(cart.BookId);
                    cart.SelectBook = book;
                    sum += book.Price * cart.Quantity; 
                }
                ViewBag.Sum = sum;
            }
            if (cartList.Count > 0)
            {
                ViewBag.Msg = "Книги в корзине";
            }
            else ViewBag.Msg = "Ваша корзина пуста. Туда надо что-то положить";

            return View(cartList);
        }
        public IActionResult Add(int id) 
        {
            string cartId;
            if(HttpContext.Request.Cookies.Keys.Count > 0 &&
               HttpContext.Request.Cookies.ContainsKey("cartId"))
            {
                cartId = HttpContext.Request.Cookies["cartId"];
            }
            else 
            { 
                cartId = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("cartId", cartId);
            }
            //check if book already exists in cart
            var query =db.ShoppingCarts.Where(c=>c.CartId == cartId && c.BookId == id).ToList();
            if(query.Count > 0) 
            { 
                CartItem item = query[0];
                item.Quantity++;
                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else 
            { 
                var item = new CartItem()
                {
                    BookId =id,
                    CartId = cartId ,
                    Quantity =1
                };
                db.ShoppingCarts.Add(item); 
            }
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Delete(int id) 
        {
            var cartItem = db.ShoppingCarts.Find(id);
            if (cartItem != null)
            {
                db.ShoppingCarts.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
