using AspNetCoreHero.ToastNotification.Abstractions;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Diagnostics;
using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly CHBHTHContext _context;
        public INotyfService _notyfService { get; }


        public HomeController(CHBHTHContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 6;
            var SanPhams = _context.SanPhams
                .AsNoTracking()
                .OrderBy(x => x.TenSp);
            PagedList<SanPham> models = new PagedList<SanPham>(SanPhams, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;



            return View(models);
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

		[Route("lien-he.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}