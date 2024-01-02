using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class AboutController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
