using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
