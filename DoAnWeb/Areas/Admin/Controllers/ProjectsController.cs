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
    public class ProjectsController : Controller
    {
        private readonly DoAnWebContext _context;

        public ProjectsController(DoAnWebContext context)
        {
            _context = context;
        }

        // GET: Admin/Projects
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            // Truy vấn Projects với phân trang
            var pagedList = await _context.Projects
                .Include(p => p.CategoryP)
                .OrderByDescending(p => p.CreatedDate)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Admin/Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.CategoryP)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Admin/Projects/Create
        public IActionResult Create()
        {
            ViewData["CategoryPid"] = new SelectList(_context.CategoryProjects, "CategoryPid", "Name");
            var defaultProject = new Project
            {
                CreatedDate = DateTime.Now
            };
            return View(defaultProject);
        }

        // POST: Admin/Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,IsActive,Image,CategoryPid,CreatedDate,Title,Detail,Client,Link")] Project project)
        {
            try
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dự án mới đã thêm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi theo nhu cầu của bạn
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm dự án. Vui lòng thử lại.";
            }
            ViewData["CategoryPid"] = new SelectList(_context.CategoryProjects, "CategoryPid", "Name", project.CategoryPid);
            return View(project);
        }

        // GET: Admin/Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CategoryPid"] = new SelectList(_context.CategoryProjects, "CategoryPid", "Name", project.CategoryPid);
            return View(project);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,IsActive,Image,CategoryPid,CreatedDate,Title,Detail,Client,Link")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thông tin dự án đã chỉnh sửa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi chỉnh sửa thông tin dự án. Vui lòng thử lại.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryPid"] = new SelectList(_context.CategoryProjects, "CategoryPid", "Name", project.CategoryPid);
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.CategoryP)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project != null)
                {
                    _context.Projects.Remove(project);
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

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
