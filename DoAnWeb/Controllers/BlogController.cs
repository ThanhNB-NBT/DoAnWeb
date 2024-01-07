using DoAnWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Configuration;

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
        public IActionResult Index(int? page, string searchString, int? id)
        {
            ViewBag.Keyword = searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 4; // Set the desired number of items per page

            var blogsQuery = _context.Blogs.Where(i => i.IsActive);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                blogsQuery = blogsQuery.Where(b => b.Title.ToLower().Contains(searchString));
            }

            // Sắp xếp theo BlogId giảm dần
            var blogs = blogsQuery.OrderByDescending(i => i.BlogId).ToPagedList(pageNumber, pageSize);

            ViewBag.blogComment = _context.BlogComments.Where(i => i.BlogId == id).ToList();

            return View(blogs);
        }


        // GET: BlogController/Details/5
        [Route("/blog-{slug}-{id:}.html", Name ="blogDetail")]
        public ActionResult Details(int? id, BlogComment blogComment)
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
            blogComment.IsActive = true;
            blogComment.CreatedDate = DateTime.Now;
            blogComment.BlogId = id;
            _context.BlogComments.Add(blogComment);
            _context.SaveChanges();
            ViewBag.blogComment = _context.BlogComments.Where(i => i.BlogId == id).ToList();
            return View(blogdetails);
        }
    }
}
