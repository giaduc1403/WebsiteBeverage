using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebsiteBeverageDemo.Repository;

namespace WebsiteBeverageDemo.ViewComponents
{
    public class LoaiHangMenuViewComponent : ViewComponent
    {
        private readonly ILoaiHangRepository _loaiHang;
        public LoaiHangMenuViewComponent(ILoaiHangRepository loaiHangRepository)
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
