using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiHangsController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }
        public LoaiHangsController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/LoaiHangs
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsLoaiHangs = _context.LoaiHangs
                .AsNoTracking()
                .OrderBy(x => x.MaLoai);
            PagedList<LoaiHang> models = new PagedList<LoaiHang>(lsLoaiHangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/LoaiHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoaiHangs == null)
            {
                return NotFound();
            }

            var loaiHang = await _context.LoaiHangs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiHang == null)
            {
                return NotFound();
            }

            return View(loaiHang);
        }

        // GET: Admin/LoaiHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai")] LoaiHang loaiHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiHang);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(loaiHang);
        }

        // GET: Admin/LoaiHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoaiHangs == null)
            {
                return NotFound();
            }

            var loaiHang = await _context.LoaiHangs.FindAsync(id);
            if (loaiHang == null)
            {
                return NotFound();
            }
            return View(loaiHang);
        }

        // POST: Admin/LoaiHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,TenLoai")] LoaiHang loaiHang)
        {
            if (id != loaiHang.MaLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiHang);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa loại sản phẩm thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiHangExists(loaiHang.MaLoai))
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
            return View(loaiHang);
        }

        // GET: Admin/LoaiHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoaiHangs == null)
            {
                return NotFound();
            }

            var loaiHang = await _context.LoaiHangs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiHang == null)
            {
                return NotFound();
            }

            return View(loaiHang);
        }

        // POST: Admin/LoaiHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiHangs == null)
            {
                return Problem("Entity set 'CHBHTHContext.LoaiHangs'  is null.");
            }
            var loaiHang = await _context.LoaiHangs.FindAsync(id);
            if (loaiHang != null)
            {
                _context.LoaiHangs.Remove(loaiHang);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiHangExists(int id)
        {
          return (_context.LoaiHangs?.Any(e => e.MaLoai == id)).GetValueOrDefault();
        }
    }
}
