using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebsiteBeverageDemo.Helper;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSanPhamsController : Controller
    {
        private readonly CHBHTHContext _context;

        public INotyfService _notyfService { get; }
        public AdminSanPhamsController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }


        // GET: Admin/AdminSanPhams
        public IActionResult Index(int page = 1, int MaL = 0)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<SanPham> lsSanPham = new List<SanPham>();
            if (MaL != 0)
            {
                lsSanPham = _context.SanPhams
                .AsNoTracking()
                .Where(x => x.MaLoai == MaL)
                .Include(x => x.MaLoaiNavigation)
                .OrderBy(x => x.MaSp).ToList();
            }
            else
            {
                lsSanPham = _context.SanPhams
                .AsNoTracking()
                .Include(x => x.MaLoaiNavigation)
                .OrderBy(x => x.MaSp).ToList();
            }



            PagedList<SanPham> models = new PagedList<SanPham>(lsSanPham.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = MaL;
            ViewBag.CurrentPage = pageNumber;


            ViewData["DanhMuc"] = new SelectList(_context.LoaiHangs, "MaLoai", "TenLoai");
            return View(models);
        }
        public IActionResult Filtter(int MaL = 0)
        {
            var url = $"/Admin/AdminSanPhams?MaL={MaL}";
            if (MaL == 0)
            {
                url = $"/Admin/AdminSanPhams";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .Include(s => s.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: Admin/AdminSanPhams/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.LoaiHangs, "MaLoai", "TenLoai");
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc");
            return View();
        }

        // POST: Admin/AdminSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,TenSp,GiaBan,Soluong,MoTa,MaLoai,MaNcc,AnhSp")] SanPham sanPham, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                sanPham.TenSp = Utilities.ToTitleCase(sanPham.TenSp);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(sanPham.TenSp) + extension;
                    sanPham.AnhSp = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(sanPham.AnhSp)) sanPham.AnhSp = "default.jpg";
                sanPham.TenSp = Utilities.ToTitleCase(sanPham.TenSp);

                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc", sanPham.MaNcc);
            return View(sanPham);
        }

        // GET: Admin/AdminSanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc", sanPham.MaNcc);
            return View(sanPham);
        }

        // POST: Admin/AdminSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSp,TenSp,GiaBan,Soluong,MoTa,MaLoai,MaNcc,AnhSp")] SanPham sanPham, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSp))
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
            ViewData["DanhMuc"] = new SelectList(_context.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "TenNcc", sanPham.MaNcc);
            return View(sanPham);
        }

        // GET: Admin/AdminSanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaLoaiNavigation)
                .Include(s => s.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: Admin/AdminSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPhams == null)
            {
                return Problem("Entity set 'CHBHTHContext.SanPhams'  is null.");
            }
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return (_context.SanPhams?.Any(e => e.MaSp == id)).GetValueOrDefault();
        }
    }
}
