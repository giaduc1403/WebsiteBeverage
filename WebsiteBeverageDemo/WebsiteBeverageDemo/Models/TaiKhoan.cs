using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int MaNguoiDung { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? Dienthoai { get; set; }
        public string? Matkhau { get; set; }
        public int? Idquyen { get; set; }
        public string? Diachi { get; set; }

        public virtual PhanQuyen? IdquyenNavigation { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
