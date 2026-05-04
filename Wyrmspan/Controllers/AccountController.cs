using Microsoft.AspNetCore.Mvc;
using Wyrmspan.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcMovie.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _authService;
    private readonly AppDbContext _db;

    /**
    Constructor for the AccountController class.
    
    Parameters:
    authService: the AuthService to use for user validation and registration.
    db: the AppDbContext to use for database operations.
    */
    public AccountController(AuthService authService, AppDbContext db)
    {
        _authService = authService;
        _db = db;
    }

    public IActionResult Login() => View();

    /*
    Handles the POST request for user login. It validates the user's credentials using the AuthService, updates the user's played count, and sets up the authentication cookie with the user's claims.
    
    Parameters:
    model: the LoginViewModel containing the username and password entered by the user.
    
    Return:
    An IActionResult that redirects to the home page if login is successful, or returns the login view
    with an error message if login fails.
    */
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

    /*
    Handles the POST request for user registration. It attempts to register a new user using the Auth
    Service

    Parameters:
    model: the RegisterViewModel containing the username, password, and profile picture URL entered by the user.
    
    Return:
    An IActionResult that redirects to the login page if registration is successful, or returns the registration view
    with an error message if registration fails.
    */
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

    /*
    Handles the POST request for user logout. It signs the user out of the authentication scheme and
    redirects them to the login page.
    */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Login");
    }

    /*
    Handles the GET request for editing the user's profile. It retrieves the current user's information from the
    database and populates an EditProfileViewModel with the user's username and profile picture URL, which is then passed to the view for rendering.
    
    Return:
    An IActionResult that returns the EditProfile view with the user's current profile information.
    */
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

    /*
    Handles the POST request for editing the user's profile. It updates the user's information in the database
    based on the data submitted in the EditProfileViewModel, and then updates the authentication cookie with the new claims if necessary.

    Parameters:
    model: the EditProfileViewModel containing the updated username, password, and profile picture URL entered
    by the user.
    
    Return:
    An IActionResult that redirects to the home page if the profile update is successful, or returns the EditProfile view with an error message if the update fails.
    */
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

    /*
    Handles the GET request for the user's profile page. It retrieves the current user's information from the database and populates a ViewProfileViewModel with the user's username, profile picture URL, and played
    count, which is then passed to the view for rendering.

    Return:
    An IActionResult that returns the Profile view with the user's profile information, or redirects to the login page if the user is not found.
    */
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