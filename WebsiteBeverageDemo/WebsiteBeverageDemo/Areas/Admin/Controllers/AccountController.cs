using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteBeverageDemo.Extension;
using WebsiteBeverageDemo.Models;
using WebsiteBeverageDemo.ModelViews;

namespace WebsiteBeverageDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
            if (taikhoanID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    TaiKhoan kh = _context.TaiKhoans
                    .Include(p => p.IdquyenNavigation)
                    .SingleOrDefault(p => p.Email.ToLower().Trim() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                    }
                    string pass = (model.Password.Trim()).ToMD5();
                    // + kh.Salt.Trim()
                    if (kh.Matkhau.Trim() != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    //đăng nhập thành công

                    //ghi nhận thời gian đăng nhập
                    _context.Update(kh);
                    await _context.SaveChangesAsync();


                    var taikhoanID = HttpContext.Session.GetString("MaNguoiDung");
                    //identity
                    //luuw seccion Makh
                    HttpContext.Session.SetString("MaNguoiDung", kh.MaNguoiDung.ToString());

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.HoTen),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("MaNguoiDung", kh.MaNguoiDung.ToString()),
                        new Claim("Idquyen", kh.Idquyen.ToString()),
                        new Claim(ClaimTypes.Role, kh.IdquyenNavigation.TenQuyen)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);



                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
        }
        [Route("logout.html", Name = "Logout")]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("MaNguoiDung");
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
        }
    }
}
