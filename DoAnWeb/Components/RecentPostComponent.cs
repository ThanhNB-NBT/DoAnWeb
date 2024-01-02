using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "RecentPost")]
    public class RecentPostComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;
        public RecentPostComponent(DoAnWebContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofPost = (from p in _context.Blogs
                              where (p.IsActive == true)
                              orderby p.BlogId descending
                              select p).Take(3).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofPost));
        }
    }
}
