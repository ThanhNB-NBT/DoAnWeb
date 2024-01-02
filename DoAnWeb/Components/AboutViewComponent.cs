using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;

namespace DoAnWeb.Components
{ 
    [ViewComponent(Name = "AboutView")]
    public class AboutViewComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;
        public AboutViewComponent(DoAnWebContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofAbout = await _context.Abouts.Where(a => a.IsActive).ToListAsync();
            var listofMemberMessage = await _context.Members.Where(m => m.IsActive).ToListAsync();

            ViewData["Abouts"] = listofAbout;
            ViewData["Members"] = listofMemberMessage;

            return View();

        }
        

    }
}
