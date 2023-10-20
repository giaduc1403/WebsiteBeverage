using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly CHBHTHContext _context;
        public ProductController(CHBHTHContext context)
        {
            _context = context;
        }
        [Route("shop.html", Name = ("ShopProduct"))]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 9;
                var lsTinDangs = _context.SanPhams
                    .AsNoTracking()
                    .OrderByDescending(x => x.MaLoai);
                PagedList<SanPham> models = new PagedList<SanPham>(lsTinDangs, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [Route("danhmuc/{TenLoai}", Name = ("ListProduct"))]
        public IActionResult List(string TenLoai, int page = 1)
        {
            try
            {
                var pageSize = 9;
                var danhmuc = _context.LoaiHangs.AsNoTracking().SingleOrDefault(x => x.TenLoai == TenLoai);

                var lsTinDangs = _context.SanPhams
                    .AsNoTracking()
                    .Where(x => x.MaLoai == danhmuc.MaLoai)
                    .OrderByDescending(x => x.MaLoai);
                PagedList<SanPham> models = new PagedList<SanPham>(lsTinDangs, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [Route("/{TenSp}-{id}.html", Name = ("SanPhamDetails"))]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.SanPhams.Include(x => x.MaLoaiNavigation).FirstOrDefault(x => x.MaSp == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsProducts = _context.SanPhams
                    .AsNoTracking()
                    .Where(x => x.MaLoai == product.MaLoai && x.MaSp != id)
                    .Take(4)
                    .OrderByDescending(x => x.MaSp)
                    .ToList();
                ViewBag.SanPham = lsProducts;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        public IActionResult SanPhamTheoLoai(int? maLoai, int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = _context.SanPhams.AsNoTracking().Where
                (x => x.MaLoai == maLoai).OrderBy(x => x.TenSp);
            PagedList<SanPham> model = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize); ;

            ViewBag.CurrentPage = pageNumber;
            return View(model);
        }

    }
}
