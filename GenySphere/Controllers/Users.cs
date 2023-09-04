using Microsoft.AspNetCore.Mvc;

namespace GeneSphere.Controllers;

public class Users : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}