using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int MaSp { get; set; }
        public string? TenSp { get; set; }
        public decimal? GiaBan { get; set; }
        public int? Soluong { get; set; }
        public string? MoTa { get; set; }
        public int? MaLoai { get; set; }
        public int? MaNcc { get; set; }
        public string? AnhSp { get; set; }

        public virtual LoaiHang? MaLoaiNavigation { get; set; }
        public virtual NhaCungCap? MaNccNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
