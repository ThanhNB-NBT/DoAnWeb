using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using X.PagedList;
using DoAnWeb.Utilities;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private readonly DoAnWebContext _context;

        public MembersController(DoAnWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Members
        public async Task<IActionResult> Index(int? page)
        {
            if (!Functions.IsLogin())
                return RedirectToAction("Index", "Login");
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var members = await _context.Members.ToPagedListAsync(pageNumber, pageSize);
            return View(members);
        }

        // GET: Admin/Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Admin/Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Name,WorkPosition,Detail,IsActive,Link,Image,Message")] Member member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(member);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thành viên mới đã thêm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log lỗi hoặc xử lý lỗi theo nhu cầu của bạn
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm thành viên. Vui lòng thử lại.";
                }
            }
            return View(member);
        }

        // GET: Admin/Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Admin/Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Name,WorkPosition,Detail,IsActive,Link,Image,Message")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thông tin thành viên đã chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi chỉnh sửa thông tin. Vui lòng thử lại.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Admin/Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Admin/Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var member = await _context.Members.FindAsync(id);
                if (member != null)
                {
                    _context.Members.Remove(member);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Dự án đã được xóa thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dự án. Vui lòng thử lại.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
