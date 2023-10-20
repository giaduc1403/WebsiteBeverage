using Microsoft.AspNetCore.Mvc;
using WebsiteBeverageDemo.Repository;

namespace WebsiteBeverageDemo.ViewComponents
{
    public class LoaiHangShopViewComponent : ViewComponent
    {
        private readonly ILoaiHangRepository _loaiHang;
        public LoaiHangShopViewComponent(ILoaiHangRepository loaiHangRepository)
        {
            _loaiHang = loaiHangRepository;
        }
        public IViewComponentResult Invoke()
        {
            var loaihang = _loaiHang.GetAllLoaiHang().OrderBy(x => x.TenLoai);
            return View(loaihang);
        }

    }
}
