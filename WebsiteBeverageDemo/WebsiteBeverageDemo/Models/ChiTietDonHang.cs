using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class ChiTietDonHang
    {
        public int CtmaDon { get; set; }
        public int MaDon { get; set; }
        public int MaSp { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? ThanhTien { get; set; }
        public int? PhuongThucThanhToan { get; set; }

        public virtual DonHang MaDonNavigation { get; set; } = null!;
        public virtual SanPham MaSpNavigation { get; set; } = null!;
    }
}
