using BookShop24.Models;
using BookShop24.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop24.Controllers
{
    public class OrderController : Controller
    {
        BookContext db;
        public OrderController(BookContext context) 
        { 
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            string cartId = GetCookie();
            if (cartId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Order order = new Order()
            {
                LastName = "Введите фамилию",
                Name = "Введите имя",
                Status = "подготовлен",
                Date = DateTime.Now,
                TotalPrice = 0,
                Address = string.Empty,
                DeliveryMethod = "курьером"
            };
            var orderItems = new List<OrderItem>();
            List<CartItem> cartList = db.ShoppingCarts.Where(c => c.CartId == cartId).ToList();
            int sum = 0;
            foreach (var cart in cartList)
            {
                var book = db.Books.Find(cart.BookId);
                if (book == null)
                    continue;
                var orderItem = new OrderItem()
                {
                    BookId = book.Id,
                    Quantity = cart.Quantity < book.Number ? cart.Quantity : book.Number,
                    TheBook = book
                };
                orderItems.Add(orderItem);
                sum += book.Price * orderItem.Quantity;
                book.Number -= orderItem.Quantity;
                db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            order.Items = orderItems;
            order.TotalPrice = sum;
            db.Orders.Add(order);
            db.Items.AddRange(orderItems);
            db.SaveChanges();
            ViewBag.PickUpPoints = new SelectList( GetPickUpPoints(),"Id","Address");
            ViewBag.DeliveryMethod = new SelectList(GetDeliveries(), "Name", "Name");

            return View(order);
        }

        private string GetCookie()
        {
            string cartId=null;
            if (HttpContext.Request.Cookies.Keys.Count > 0 &&
               HttpContext.Request.Cookies.ContainsKey("cartId"))
            {
                cartId = HttpContext.Request.Cookies["cartId"];
            }

            return cartId;
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            if(order.PickUpPointId > 0)
            {
                order.Address= GetPickUpPoints()[order.PickUpPointId].Address;
            }
            order.Status = "подтвержден";
            db.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            // remove cart items = clear the cart
            string cartId = GetCookie();
            if(cartId != null) 
            { 
                var carts = db.ShoppingCarts.Where(c=> c.CartId == cartId).ToList();
                db.RemoveRange(carts);
                db.SaveChanges();
                HttpContext.Response.Cookies.Delete("cartId");
            }
            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            ViewBag.Msg = "Ваш заказ подтвержден и скоро к вам приедет";
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var order = db.Orders.Find(id);
            var orderItems = db.Items.Where(i=> i.OrderId == id).ToList();
            foreach(var item in orderItems)
            {
                var book = db.Books.Find(item.BookId);
                if (book == null)
                    continue;
                book.Number += item.Quantity;
                db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            order.Items = orderItems;
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        List<PickUpPoint> GetPickUpPoints()
        {
            return new List<PickUpPoint>() {
                new PickUpPoint() { Id = 0, Address ="Выберите пункт самовывоза"},
                new PickUpPoint() { Id = 1, Address ="Озон, Ленинградский пр-т, 52"},
                new PickUpPoint() { Id = 2, Address ="Яндекс, ул. Клубная, 18"},
                new PickUpPoint() { Id = 3, Address ="Озон, пр-т Ленина, 24"}
            };
        }
        List<Delivery> GetDeliveries()
        {
            return new List<Delivery>() {
                new Delivery() { Id = 0, Name ="Выберите способ доставки"},
                new Delivery() { Id = 1, Name ="самовывоз"},
                new Delivery() { Id = 2, Name ="курьер"}
            };
        }

    }
    internal class PickUpPoint
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }
    internal class Delivery
    {
        public int Id { get; set; }
        public string Name { get; set; }    

    }



}
