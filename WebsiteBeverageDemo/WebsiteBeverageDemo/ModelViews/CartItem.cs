using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.ModelViews
{
    public class CartItem
    {
        public SanPham sanPham{ get; set; }
        public int soLuong { get; set; }
        public decimal? ThanhTien => soLuong * sanPham.GiaBan.Value;
    }
}
