using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "BlogView")]
    public class BlogViewComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;
        public BlogViewComponent(DoAnWebContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.Blogs
                .Where(m => m.IsActive)
                .OrderByDescending(i => i.BlogId)
                .ToListAsync();

            ViewBag.blogComment = _context.BlogComments.Where(m => m.IsActive).ToList();

            return View(items);
        }
    }
}
