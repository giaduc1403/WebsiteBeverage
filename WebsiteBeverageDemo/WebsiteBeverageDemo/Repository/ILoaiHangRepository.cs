using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Repository
{
    public interface ILoaiHangRepository
    {
        LoaiHang Add(LoaiHang loaiHang);

        LoaiHang Update(LoaiHang loaiHang);

        LoaiHang Delete(String maloaiHang);

        LoaiHang Get(String maloaiHang);

        IEnumerable<LoaiHang> GetAllLoaiHang();
    }
}
