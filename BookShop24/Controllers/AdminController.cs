using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookShop24.ViewModels;


namespace BookShop24.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> EditUserRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            var model = new ChangeRoleViewModel()
            {
                UserId = user.Id,
                UserEmail = user.Email,
                AllRoles = roles,
                UserRoles = userRoles
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRole(string id, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removeRoles = userRoles.Except(roles);
            await _userManager.RemoveFromRolesAsync(user, removeRoles);
            await _userManager.AddToRolesAsync(user, addedRoles);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        public IActionResult GetRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string name)
        {
            if (await _roleManager.FindByNameAsync(name) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(name));
            }
            return RedirectToAction("GetRoles");
        }
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("GetRoles");
        }
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            await _roleManager.UpdateAsync(role);
            return RedirectToAction("GetRoles");
        }
    }
}
