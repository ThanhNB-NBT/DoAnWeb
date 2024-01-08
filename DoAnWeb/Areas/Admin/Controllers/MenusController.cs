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
    public class MenusController : Controller
    {
        private readonly DoAnWebContext _context;

        public MenusController(DoAnWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Menus
        public async Task<IActionResult> Index(int? page)
        {
            if (!Functions.IsLogin())
                return RedirectToAction("Index", "Login");
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var menus = await _context.Menus.ToPagedListAsync(pageNumber, pageSize);

            foreach (var menu in menus)
            {
                if (menu.ParentId == null || menu.ParentId == 0)
                {
                    menu.ParentName = "Không có";
                }
                else
                {
                    menu.ParentName = _context.Menus.FirstOrDefault(m => m.MenuId == menu.ParentId)?.MenuName ?? string.Empty;
                }
            }


            return View(menus);
            //return View(await _context.Menus.ToListAsync());
        }

        // GET: Admin/Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }
            menu.ParentName = _context.Menus.FirstOrDefault(m => m.MenuId == menu.ParentId)?.MenuName ?? string.Empty;
            return View(menu);
        }

        // GET: Admin/Menus/Create
        public IActionResult Create()
        {
            ViewBag.ParentMenus = new SelectList(_context.Menus, "MenuId", "MenuName");
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuName,IsActive,ControllerName,ActionName,Levels,ParentId,Link,MenuOrder,Position")] Menu menu)
        {
            try
            {
               _context.Add(menu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Menu mới đã thêm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi theo nhu cầu của bạn
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm Menu. Vui lòng thử lại.";
            }
            return View(menu);
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewBag.ParentMenus = new SelectList(_context.Menus, "MenuId", "MenuName");
            return View(menu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,MenuName,IsActive,ControllerName,ActionName,Levels,ParentId,Link,MenuOrder,Position")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thông tin Menu đã chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
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
            return View(menu);
        }

        // GET: Admin/Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var menu = await _context.Menus.FindAsync(id);
                if (menu != null)
                {
                    _context.Menus.Remove(menu);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Menu đã được xóa thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa Menu. Vui lòng thử lại.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MenuId == id);
        }
    }
}
