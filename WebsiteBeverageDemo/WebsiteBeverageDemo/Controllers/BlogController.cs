using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Controllers
{
    public class BlogController : Controller
    {
        private readonly CHBHTHContext _context;
        public BlogController(CHBHTHContext context)
        {
            _context = context;
        }
        [Route("blogs.html", Name = ("Blog"))]
        public IActionResult Index(int? page,int Matt)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsTinDangs = _context.TinTucs
                .AsNoTracking()
                .OrderBy(x => x.MaTt);
            PagedList<TinTuc> models = new PagedList<TinTuc>(lsTinDangs, pageNumber, pageSize);
            ViewBag.CurrentMaTt = Matt;
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinChiTiet")]
        public IActionResult Details(int id)
        {
            var tindang = _context.TinTucs.AsNoTracking().SingleOrDefault(x => x.MaTt == id);
            if (tindang == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaivietlienquan = _context.TinTucs
                .AsNoTracking()
                .Where(x => x.MaTt != id)
                .Take(3)
                .OrderByDescending(x => x.NoiDung).ToList();
            ViewBag.Baivietlienquan = lsBaivietlienquan;
            return View(tindang);
        }
    }
}
