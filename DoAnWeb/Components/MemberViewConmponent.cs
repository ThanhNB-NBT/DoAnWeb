using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "MemberView")]
    public class MemberViewComponent : ViewComponent
    {
        private readonly DoAnWebContext _context;
        public MemberViewComponent(DoAnWebContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofMember = (from m in _context.Members
                                where (m.IsActive == true)
                                select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofMember));
        }
    }
}