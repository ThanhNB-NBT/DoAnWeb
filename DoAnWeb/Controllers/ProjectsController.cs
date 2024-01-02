using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace WebDoAn.Controllers
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
        public IActionResult Index()
        {
            var projects = _context.Projects.Where(m => m.IsActive).OrderByDescending(i => i.ProjectId).ToList();
            return View(projects);
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
           
            return View(Details);
        }
    }
}
