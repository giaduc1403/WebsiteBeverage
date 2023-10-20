using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBeverageDemo.Extension;
using WebsiteBeverageDemo.Helper;
using WebsiteBeverageDemo.Models;
using WebsiteBeverageDemo.ModelViews;

namespace WebsiteBeverageDemo.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
            MuaHangModelView model = new MuaHangModelView();
            if (taikhoanID != null)
            {
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.MaNguoiDung;
                model.FullName = khachhang.HoTen;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Dienthoai;
                model.Address = khachhang.Diachi;
            }
            ViewBag.GioHang = cart;
            return View(model);
        }
        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangModelView muaHang)
        {
            //Lay ra gio hang de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
            MuaHangModelView model = new MuaHangModelView();
            if (taikhoanID != null)
            {
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.MaNguoiDung;
                model.FullName = khachhang.HoTen;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Dienthoai;
                model.Address = khachhang.Diachi;

                //_context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                    //Khoi tao don hang
                    DonHang donhang = new DonHang();
                    donhang.MaNguoiDung = model.CustomerId;
                    donhang.DiaChiNhanHang = model.Address;

                    donhang.NgayDat = DateTime.Now;
                    donhang.TinhTrang = 1;//Don hang moi
                    donhang.ThanhToan = Convert.ToInt32(cart.Sum(x => x.ThanhTien));
                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang

                    foreach (var item in cart)
                    {
                        ChiTietDonHang orderDetail = new ChiTietDonHang();
                        orderDetail.MaDon = donhang.Madon;
                        orderDetail.MaSp = item.sanPham.MaSp;
                        orderDetail.SoLuong = item.soLuong;
                        orderDetail.ThanhTien = donhang.ThanhToan;
                        orderDetail.DonGia = item.sanPham.GiaBan;
                        //orderDetail.MaDonNavigation.NgayDat = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //clear gio hang
                    HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    _notyfService.Success("Đơn hàng đặt thành công");
                    //cap nhat thong tin khach hang
                    return RedirectToAction("Success");


            }
            catch
            {
               
                ViewBag.GioHang = cart;
                return View(model);
            }
            ViewBag.GioHang = cart;
            return View(model);
        }
        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID));
                var donhang = _context.DonHangs
                    .Where(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.NgayDat)
                    .FirstOrDefault();
                MuaHangThanhCong successVM = new MuaHangThanhCong();
                successVM.FullName = khachhang.HoTen;
                successVM.DonHangID = donhang.Madon;
                successVM.Phone = khachhang.Dienthoai;
                successVM.Address = khachhang.Diachi;
              
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }

    }
}
