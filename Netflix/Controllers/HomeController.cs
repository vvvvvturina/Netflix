using System.Diagnostics;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix.Models;

namespace Netflix.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserContext _userDb;

    public HomeController(ILogger<HomeController> logger, UserContext userDb)
    {
        _logger = logger;
        _userDb = userDb;
    }

    [Authorize]
    public IActionResult Index()
    {
        return Content(User.Identity.Name);
    }

}