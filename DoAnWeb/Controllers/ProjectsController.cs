using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly DoAnWebContext _context;
        public ProjectsController(ILogger<ProjectsController> logger, DoAnWebContext context)
        {
            _logger = logger;
            _context = context;
        }
        //public IActionResult Index()
        //{
        //    var projects = _context.Projects.Where(m => m.IsActive).OrderByDescending(i => i.ProjectId).ToList();
        //    return View(projects);
        //}
        public IActionResult Index()
        {
            var projects = _context.Projects
                            .Include(p => p.CategoryP)
                            .Where(m => m.IsActive)
                            .OrderByDescending(i => i.ProjectId)
                            .ToList();

            var categories = _context.CategoryProjects.ToList();

            ViewBag.Projects = projects;
            ViewBag.Categories = categories;

            return View();
        }
        [Route("/projects-{slug}-{id:}.html", Name = "Details")]
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Details = _context.Projects.Where(i => i.ProjectId == id && i.IsActive).FirstOrDefault();
            if (Details == null)
            {
                return NotFound();
            }
            var categories = _context.CategoryProjects.ToList();
            ViewBag.Categories = categories;
            return View(Details);
        }
    }
}
