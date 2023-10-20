using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly CHBHTHContext _context;

        public SearchController(CHBHTHContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<SanPham> ls = new List<SanPham>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListSanPhamsSearchPartialView", null);
            }
            ls = _context.SanPhams.AsNoTracking()
                                  .Include(a => a.MaLoaiNavigation)
                                  .Where(x => x.TenSp.Contains(keyword))
                                  .OrderByDescending(x => x.TenSp)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListSanPhamsSearchPartialView", null);
            }
            else
            {
                return PartialView("ListSanPhamsSearchPartialView", ls);
            }
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
