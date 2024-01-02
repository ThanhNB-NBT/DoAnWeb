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
            var listofBlog = (from b in _context.Blogs
                              where (b.IsActive == true)
                              select b).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofBlog));
        }
    }
}
