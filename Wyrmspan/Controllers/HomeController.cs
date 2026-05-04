using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wyrmspan.Models;

namespace Wyrmspan.Controllers;

public class HomeController : Controller
{
    /*  
    This controller is responsible for handling requests to the home page and the privacy page.
    It also handles errors and displays an error page when an error occurs. 

    Return:
    An IActionResult that returns the appropriate view for each action.
    */
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    /*  
    This controller is responsible for handling requests to the privacy page.

    Return:
    An IActionResult that returns the Privacy view when the user navigates to the privacy page.
    */
    public IActionResult Privacy()
    {
        return View();
    }

    /*  
    This controller is responsible for handling requests to the error page. It creates an ErrorViewModel with the current request ID and returns the Error view.

    Return:
    An IActionResult that returns the Error view when an error occurs.
    */
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
