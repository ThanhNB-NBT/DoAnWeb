using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace DoAnWeb.Controllers
{
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;

        private readonly DoAnWebContext _context;

        public MemberController(ILogger<MemberController> logger, DoAnWebContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var members = await _context.Members.ToPagedListAsync(pageNumber, pageSize);
            return View(members);

        }
    }
}
