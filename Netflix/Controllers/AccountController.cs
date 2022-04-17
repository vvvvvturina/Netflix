using System.Security.Claims;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Netflix.Controllers;

public class AccountController : Controller
{
    private readonly UserContext _userDb;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserContext userDb, ILogger<AccountController> logger)
    {
        _userDb = userDb;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            User user = await _userDb.Users.FirstOrDefaultAsync(u =>
                u.Email == loginModel.Email && u.Password == loginModel.Password);
            if (user != null)
            {
                await Authenticate(loginModel.Email); // аутентификация
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return View(loginModel);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (ModelState.IsValid)
        {
            User user = await _userDb.Users.FirstOrDefaultAsync(u => u.Email == registerModel.Email);
            if (user == null)
            {
                _userDb.Users.Add(new User { Email = registerModel.Email, Password = registerModel.Password });
                await _userDb.SaveChangesAsync();
 
                await Authenticate(registerModel.Email);
 
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(registerModel);
    }
    
    private async Task Authenticate(string userName)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
        };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
 
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}