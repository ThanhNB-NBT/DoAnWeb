using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using X.PagedList;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly DoAnWebContext _context;

        public BlogsController(DoAnWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            // Truy vấn Projects với phân trang
            var pagedList = await _context.Blogs
                .Include(p => p.Category).Include(p => p.Account) // Kết hợp bảng có khóa ngoại
                .ToPagedListAsync(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.CategoryBlogs, "CategoryId", "Name");
            var defaultBlog = new Blog
            {
                CreatedDate = DateTime.Now
            };
            return View(defaultBlog);
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Alias,Description,Detail,Image,CreatedDate,CreatedBy,Tags,AccountId,IsActive,CategoryId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(blog);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Dự án mới đã thêm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log lỗi hoặc xử lý lỗi theo nhu cầu của bạn
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm dự án. Vui lòng thử lại.";
                }
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", blog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryBlogs, "CategoryId", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", blog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryBlogs, "CategoryId", "Name", blog.CategoryId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Alias,Description,Detail,Image,CreatedDate,CreatedBy,Tags,AccountId,IsActive,CategoryId")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thông tin blog đã chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi chỉnh sửa thông tin blog. Vui lòng thử lại.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "FullName", blog.AccountId);
            ViewData["CategoryId"] = new SelectList(_context.CategoryBlogs, "CategoryId", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Account)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var blog = await _context.Blogs.FindAsync(id);
                if (blog != null)
                {
                    _context.Blogs.Remove(blog);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Blog đã được xóa thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa blog. Vui lòng thử lại.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
