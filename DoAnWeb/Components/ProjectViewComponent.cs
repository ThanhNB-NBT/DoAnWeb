using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Components
{
    [ViewComponent(Name ="ProjectView")]
    public class ProjectViewComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;

        public ProjectViewComponent(DoAnWebContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofProject = (from p in _context.Projects
                              where (p.IsActive == true)
                              select p).ToList();
           
            return await Task.FromResult((IViewComponentResult)View("Default", listofProject));
        }
    }
}
