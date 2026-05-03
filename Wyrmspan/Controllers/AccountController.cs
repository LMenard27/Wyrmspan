using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Schema;
using Wyrmspan.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Data.Common;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcMovie.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _authService;
    private readonly AppDbContext _db;

    public AccountController(AuthService authService, AppDbContext db)
    {
        _authService = authService;
        _db = db;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = _authService.ValidateUser(model.Username, model.Password);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login");
            return View(model);
        }

        user.Played++;
        _db.SaveChanges();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Pfp", user.Pfp ?? "/images/default.png")
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register() => View();
    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {

        var result = _authService.Register(model.Username, model.Password, model.Pfp);

        if (result == null)
        {
            ModelState.AddModelError("", "User already exists");
            return View(model);
        }

        return RedirectToAction("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        var username = User.Identity.Name;

        var user = _db.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return RedirectToAction("Login");

        var model = new EditProfileViewModel
        {
            Username = user.Username,
            Pfp = user.Pfp
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);

        if (user == null)
            return RedirectToAction("Login");

        // Update DB
        user.Username = model.Username;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
        }

        user.Pfp = string.IsNullOrWhiteSpace(model.Pfp)
            ? user.Pfp
            : model.Pfp;

        _db.SaveChanges();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Pfp", user.Pfp ?? "/images/default.png")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var username = User.Identity.Name;

        var user = _db.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return RedirectToAction("Login");

        var model = new ViewProfileViewModel
        {
            Username = user.Username,
            Pfp = user.Pfp ?? "/images/default.png",
            Played = user.Played,
        };

        return View(model);
    }
}