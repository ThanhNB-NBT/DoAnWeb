using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DoAnWeb.Utilities;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly DoAnWebContext _context;

        public LoginController(DoAnWebContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = _context.Accounts
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.UserName == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password) && user.Role != null)
            {
                if (!user.IsActive)
                {
                    TempData["ErrorMessage"] = "Tài khoản bị khóa";
                }
                else
                {
                    if (user.Role.RoleName == "User")
                    {
                        TempData["ErrorMessage"] = "Bạn không đủ quyền hạn";
                    }
                    else
                    {
                        // Lưu các thay đổi vào cơ sở dữ liệu
                        //_context.SaveChanges();
                        Functions._AccountId = user.AccountId;
                        Functions._UserName = user.UserName;
                        Functions._FullName = user.FullName;
                        Functions._Email = user.Email;
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Tên người dùng hoặc mật khẩu không đúng";
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Functions._AccountId = 0;
            Functions._Email = string.Empty;
            Functions._UserName = string.Empty;
            return RedirectToAction("Index", "login");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp tên đăng nhập
                if (_context.Accounts.Any(u => u.UserName == model.UserName))
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại!";
                }
                // Kiểm tra trùng lặp email
                else if (_context.Accounts.Any(u => u.Email == model.Email))
                {
                    TempData["ErrorMessage"] = "Email đã được sử dụng!";
                }
                else
                {
                    var newAccount = new Account
                    {
                        UserName = model.UserName,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        FullName = model.FullName,
                        Phone = model.Phone,
                        Email = model.Email,
                        RoleId = 1,
                        IsActive = true
                    };

                    _context.Accounts.Add(newAccount);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Tài khoản đã được tạo thành công, hãy đăng nhập!";
                    return RedirectToAction("Index", "Login");
                }
            }

            return RedirectToAction("Register", "Login");
        }

    }
}
