using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteBeverageDemo.Extension;
using WebsiteBeverageDemo.Helper;
using WebsiteBeverageDemo.Models;
using WebsiteBeverageDemo.ModelViews;

namespace WebsiteBeverageDemo.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {

        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }

        public AccountsController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.Dienthoai.ToLower() == Phone.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Phone + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
            if (taikhoanID != null)
            {
                var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.MaNguoiDung == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.DonHangs
                        //.Include(x => x.TinhTrang)
                        .AsNoTracking()
                        .Where(x => x.MaNguoiDung == khachhang.MaNguoiDung)
                        .OrderByDescending(x => x.NgayDat)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }

            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(DangKyViewModel taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TaiKhoan khachhang = new TaiKhoan
                    {
                        HoTen = taikhoan.FullName,
                        Dienthoai = taikhoan.Phone.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        Matkhau = (taikhoan.Password).ToMD5(),
                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("MaNguoiDung", khachhang.MaNguoiDung.ToString());
                        var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");

                        //Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,khachhang.HoTen),
                            new Claim("MaNguoiDung", khachhang.MaNguoiDung.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch
                    {
                        return RedirectToAction("DangkyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel kh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(kh.UserName);
                    var khachhang = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == kh.UserName);

                    if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
                    string pass = (kh.Password).ToMD5();
                    if (khachhang.Matkhau != pass)
                    {
                        _notyfService.Success("Thông tin đăng nhập chưa chính xác");
                        return View(kh);
                    }
                    //Luu Session MaKh
                    HttpContext.Session.SetString("MaNguoiDung", khachhang.MaNguoiDung.ToString());
                    var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");

                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.HoTen),
                        new Claim("MaNguoiDung", khachhang.MaNguoiDung.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    return RedirectToAction("Dashboard", "Accounts");

                    //if (string.IsNullOrEmpty(returnUrl))
                    //{
                    //    return RedirectToAction("Dashboard", "Accounts");
                    //}
                    //else
                    //{
                    //    return Redirect(returnUrl);
                    //}
                }
            }
            catch
            {
                return RedirectToAction("DangkyTaiKhoan", "Accounts");
            }
            return View(kh);
        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaNguoiDung");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("doi-mat-khau.html", Name = "ChangePass")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.TaiKhoans.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = (model.PasswordNow.Trim()).ToMD5();
                    {
                        string passnew = (model.Password.Trim()).ToMD5();
                        taikhoan.Matkhau = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                }
            }
            catch
            {
                _notyfService.Success("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "Accounts");
            }
            _notyfService.Success("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "Accounts");
        }
    }
}
