using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookShop24.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BookShop24.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager; 

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                IdentityUser user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded) 
                { 
                    var code =await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account", 
                        new { userId = user.Id,code },
                        protocol:HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию пройдя по ссылке:<a href ='{callbackUrl}'> link</a>");
                    return Content("Для завершения регистрации проверьте почту и пройдите по ссылке," +
                        " указанной в письме");
                }
                foreach( var error in result.Errors ) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                } 
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                // установка куки
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index","Home");
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            // проверяем, принадлежит ли URL приложению
                            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неподтвержден адрес электронной почты");
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if(user==null )
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user==null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
                else
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if(result.Succeeded) 
                    {
                        ViewBag.Msg = "Ваш пароль успешно изменен";
                        return View("Success");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
