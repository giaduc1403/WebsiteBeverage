using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhanQuyensController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }
        public PhanQuyensController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/PhanQuyens
        public async Task<IActionResult> Index()
        {
              return _context.PhanQuyens != null ? 
                          View(await _context.PhanQuyens.ToListAsync()) :
                          Problem("Entity set 'CHBHTHContext.PhanQuyens'  is null.");
        }

        // GET: Admin/PhanQuyens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhanQuyens == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens
                .FirstOrDefaultAsync(m => m.Idquyen == id);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            return View(phanQuyen);
        }

        // GET: Admin/PhanQuyens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PhanQuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idquyen,TenQuyen")] PhanQuyen phanQuyen)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(phanQuyen);
                //await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(phanQuyen);
        }

        // GET: Admin/PhanQuyens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhanQuyens == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens.FindAsync(id);
            if (phanQuyen == null)
            {
                return NotFound();
            }
            return View(phanQuyen);
        }

        // POST: Admin/PhanQuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idquyen,TenQuyen")] PhanQuyen phanQuyen)
        {
            if (id != phanQuyen.Idquyen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanQuyen);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanQuyenExists(phanQuyen.Idquyen))
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
            return View(phanQuyen);
        }

        // GET: Admin/PhanQuyens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PhanQuyens == null)
            {
                return NotFound();
            }

            var phanQuyen = await _context.PhanQuyens
                .FirstOrDefaultAsync(m => m.Idquyen == id);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            return View(phanQuyen);
        }

        // POST: Admin/PhanQuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PhanQuyens == null)
            {
                return Problem("Entity set 'CCHBHTHContext.PhanQuyens'  is null.");
            }
            var phanQuyen = await _context.PhanQuyens.FindAsync(id);
            if (phanQuyen != null)
            {
                _context.PhanQuyens.Remove(phanQuyen);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool PhanQuyenExists(int id)
        {
          return (_context.PhanQuyens?.Any(e => e.Idquyen == id)).GetValueOrDefault();
        }
    }
}
