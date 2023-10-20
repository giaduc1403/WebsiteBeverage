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
    public class AdminDonHangsController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }


        public AdminDonHangsController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDonHangs
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var Orders = _context.DonHangs.Include(o => o.MaNguoiDungNavigation)
                .AsNoTracking()
                .OrderBy(x => x.NgayDat);
            PagedList<DonHang> models = new PagedList<DonHang>(Orders, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;



            return View(models);
        }

        // GET: Admin/AdminDonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.DonHangs
                .Include(o => o.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.Madon == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.ChiTietDonHangs
                .Include(x => x.MaSpNavigation)
                .AsNoTracking()
                .Where(x => x.MaDon == order.Madon)
                .OrderBy(x => x.CtmaDon)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(order);
        }

        // GET: Admin/AdminDonHangs/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoans, "MaNguoiDung", "MaNguoiDung");
            return View();
        }

        // POST: Admin/AdminDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Madon,NgayDat,TinhTrang,ThanhToan,DiaChiNhanHang,MaNguoiDung,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoans, "MaNguoiDung", "MaNguoiDung", donHang.MaNguoiDung);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DonHangs == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoans, "MaNguoiDung", "MaNguoiDung", donHang.MaNguoiDung);
            return View(donHang);
        }

        // POST: Admin/AdminDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Madon,NgayDat,TinhTrang,ThanhToan,DiaChiNhanHang,MaNguoiDung,TongTien")] DonHang donHang)
        {
            if (id != donHang.Madon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Madon))
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
            ViewData["MaNguoiDung"] = new SelectList(_context.TaiKhoans, "MaNguoiDung", "MaNguoiDung", donHang.MaNguoiDung);
            return View(donHang);
        }

        // GET: Admin/AdminDonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.DonHangs
                .Include(o => o.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.Madon == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.ChiTietDonHangs
                .Include(x => x.MaSpNavigation)
                .AsNoTracking()
                .Where(x => x.MaDon == order.Madon)
                .OrderBy(x => x.CtmaDon)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;

            return View(order);
        }

        // POST: Admin/AdminDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.DonHangs.FindAsync(id);
            var remove = _context.DonHangs.Remove(order);
            _context.Update(order);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa đơn hàng thành công");
            return RedirectToAction(nameof(Index));
            //_context.Remove(_context.TaiKhoans.Find(id));
            //_context.SaveChanges();
            //return RedirectToAction(nameof(Index));

        }

        private bool DonHangExists(int id)
        {
          return (_context.DonHangs?.Any(e => e.Madon == id)).GetValueOrDefault();
        }
    }
}
