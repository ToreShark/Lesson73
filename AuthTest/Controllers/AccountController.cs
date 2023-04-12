using System.Net;
using System.Security.Claims;
using AuthTest.Data;
using AuthTest.Models;
using AuthTest.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    // GET
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email &&
                                                                 u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(model.Email);
                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }

    private async Task Authenticate(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(id));
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
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                _db.Users.Add(new Models.User { Email = model.Email, Password = model.Password });

                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}