using BookShop24.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop24.Controllers
{
    public class ClientController : Controller
    {
        BookContext db;
        private UserManager<IdentityUser> _userManager;
        public ClientController(BookContext context, UserManager<IdentityUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> GetPurchaseHistory(string name)
        {          
                var user = await _userManager.FindByNameAsync(name);
                var orders = db.Orders.Where(ordr => ordr.ClientId== user.Id).OrderByDescending(d=>d.Date).ToList();
                ViewBag.Orders = orders;
                var books = new List<Book>();
                foreach (var order in orders)
                {
                    var items = db.Items.Where(i=> i.OrderId==order.Id).Include(b=> b.TheBook).ToList();
                    books.AddRange(items.Select(item=>item.TheBook));
                }
                books = books.Distinct().ToList();  
                return View(books);
        }


    }
}
