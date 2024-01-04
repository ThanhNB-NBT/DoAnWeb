using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;
        public AdminMenuComponent(DoAnWebContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = await _context.AdminMenus
                .Where(mn => mn.IsActive)
                .ToListAsync();

            return View("Default", mnList);
        }
    }
}
