using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class ContactController : Controller
    {
        private DoAnWebContext _context = new DoAnWebContext();

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Contact contact)
        {
            try
            {
                contact.CreatedDate = DateTime.Now;
                _context.Contacts.Add(contact);
                _context.SaveChangesAsync();
                ViewBag.success = "success";
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
