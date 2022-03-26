#nullable disable
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Data;
using MyPortfolio.Models;

namespace MyPortfolio.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("")]
    public IActionResult Index()
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);
        ViewData["Category"] = _context.Category.OrderByDescending(item => item.Id);
        return View();
    }
    [Route("Projects")]
    public async Task<IActionResult> Projects()
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);

        var applicationDbContext = _context.Project.Include(p => p.Category);
        return View(await applicationDbContext.ToListAsync());
    }

    [Route("Projects/Details/{id?}")]
    public async Task<IActionResult> Details(int? id)
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);

        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Project
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [Route("Courses")]
    public async Task<IActionResult> Courses()
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);

        return View(await _context.Course.ToListAsync());
    }

    [Route("Contact")]
    public IActionResult Contact()
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        //Includes data from database inside of ViewData acccesible in View
        ViewData["SocialMedia"] = _context.SocialMedia.OrderByDescending(item => item.Title);
        ViewData["Project"] = _context.Project.Include(p => p.Category).OrderByDescending(item => item.Created);
        ViewData["Course"] = _context.Course.OrderByDescending(item => item.StartDate);

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

