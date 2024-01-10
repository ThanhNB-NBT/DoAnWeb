using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using DoAnWeb.Utilities;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly DoAnWebContext _context;

        public AccountsController(DoAnWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            if (!Functions.IsLogin())
                return RedirectToAction("Index", "Login");
            var doAnWebContext = _context.Accounts.Include(a => a.Role);
            return View(await doAnWebContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,UserName,Password,FullName,Phone,Email,RoleId,IsActive,Image")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", account.RoleId);
            return View(account);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name", account.RoleId);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,UserName,FullName,Phone,Email,Image")] Account updatedAccount)
        {
            if (id != updatedAccount.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy tài khoản hiện tại từ cơ sở dữ liệu
                    var existingAccount = await _context.Accounts
                        .Include(a => a.Role) // Bạn có thể cần include Role nếu cần dùng RoleId
                        .FirstOrDefaultAsync(m => m.AccountId == id);

                    if (existingAccount == null)
                    {
                        return NotFound();
                    }

                    // Chỉnh sửa các trường cụ thể của đối tượng
                    existingAccount.UserName = updatedAccount.UserName;
                    existingAccount.FullName = updatedAccount.FullName;
                    existingAccount.Phone = updatedAccount.Phone;
                    existingAccount.Email = updatedAccount.Email;
                    existingAccount.Image = updatedAccount.Image;

                    // Các trường IsActive, RoleId, Password không thay đổi

                    // Cập nhật và lưu thay đổi vào cơ sở dữ liệu
                    _context.Update(existingAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(updatedAccount.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả về view với dữ liệu đầu vào
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name", updatedAccount.RoleId);
            return View(updatedAccount);
        }


        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> ChangePasswordAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            // Tạo một đối tượng ChangePassword từ dữ liệu của Account
            var changePasswordModel = new ChangePassword
            {
                AccountId = account.AccountId
                // Các trường khác có thể cần được thiết lập tùy thuộc vào yêu cầu của bạn
            };

            return View(changePasswordModel);
        }
        [HttpPost, ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mật khẩu hiện tại
                var user = await _context.Accounts.FindAsync(model.AccountId);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
                {
                    // Kiểm tra mật khẩu mới và xác nhận mật khẩu
                    if (model.NewPassword == model.ConfirmPassword)
                    {
                        // Lưu mật khẩu mới vào cơ sở dữ liệu
                        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
                        return RedirectToAction("Details", "Accounts", new { id = model.AccountId });

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Mật khẩu mới và xác nhận mật khẩu không khớp!";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng!";
                }
            }

            return View("ChangePassword", model);
        }

    }
}
