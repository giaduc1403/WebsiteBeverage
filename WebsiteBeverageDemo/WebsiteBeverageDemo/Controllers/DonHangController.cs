using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBeverageDemo.Models;
using WebsiteBeverageDemo.ModelViews;

namespace WebsiteBeverageDemo.Controllers
{
    public class DonHangController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }
        public DonHangController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
                if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID));
                if (khachhang == null) return NotFound();
                var donhang = await _context.DonHangs
                    //.Include(x => x.TinhTrang)
                    .FirstOrDefaultAsync(m => m.Madon == id && Convert.ToInt32(taikhoanID) == m.MaNguoiDung);
                if (donhang == null) return NotFound();

                var chitietdonhang = _context.ChiTietDonHangs
                    .Include(x => x.MaSpNavigation)
                    .AsNoTracking()
                    .Where(x => x.MaDon == id)
                    .OrderBy(x => x.CtmaDon)
                    .ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);

            }
            catch
            {
                return NotFound();
            }
        }
    }
}
