using DoAnWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;

        private readonly DoAnWebContext _context;

        public BlogController(ILogger<BlogController> logger, DoAnWebContext context)
        {
            _logger = logger;
            _context = context;
        }
        // GET: BlogController
        public ActionResult Index()
        {
            ViewBag.blogComment = _context.BlogComments.ToList();
            return View();
        }

        // GET: BlogController/Details/5
        [Route("/blog-{slug}-{id:}.html", Name ="blogDetail")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blogdetails = _context.Blogs.Where(i => i.BlogId == id).FirstOrDefault();
            if (blogdetails == null)
            {
                return NotFound();
            }
            ViewBag.blogComment = _context.BlogComments.Where(i => i.BlogId == id).ToList();
            return View(blogdetails);
        }
    }
}
