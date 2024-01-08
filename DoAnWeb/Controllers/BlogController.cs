using DoAnWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.Configuration;
using Azure;
using Newtonsoft.Json;

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
        public IActionResult Index(int? page, string searchString, int? id, int? categoryId, string tag)
        {
            ViewBag.Keyword = searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 3; //số lượng bài viết trong 1 trang

            var blogsQuery = _context.Blogs.Where(i => i.IsActive);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                blogsQuery = blogsQuery.Where(b => b.Title.ToLower().Contains(searchString));
            }
            if (categoryId.HasValue)
            {
                blogsQuery = blogsQuery.Where(b => b.CategoryId == categoryId);
            }
            if (!string.IsNullOrEmpty(tag))
            {
                blogsQuery = blogsQuery.Where(b => b.Tags.Contains(tag));
            }
            // Sắp xếp theo BlogId giảm dần
            var blogs = blogsQuery.OrderByDescending(i => i.CreatedDate).ToPagedList(pageNumber, pageSize);
            var categoriesWithCount = _context.CategoryBlogs
                                 .Where(c => c.IsActive)
                                 .Select(c => new
                                 {
                                     Category = c,
                                     BlogCount = c.Blogs.Count(b => b.IsActive)
                                 })
                                 .ToList();

            ViewBag.CategoriesWithCount = categoriesWithCount;

            var tags = _context.Blogs
                        .Where(b => b.IsActive && b.Tags != null)
                        .AsEnumerable() // Chuyển đổi kết quả truy vấn thành một tập hợp
                        .Select(b => b.Tags.Split(new char[] { ',' }))
                        .SelectMany(tagsArray => tagsArray)
                        .Distinct()
                        .ToList();


            ViewBag.Tags = tags;

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
