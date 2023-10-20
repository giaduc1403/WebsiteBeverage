using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class DonHang
    {
        public DonHang()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        public int Madon { get; set; }
        public DateTime? NgayDat { get; set; }
        public int? TinhTrang { get; set; }
        public int? ThanhToan { get; set; }
        public string? DiaChiNhanHang { get; set; }
        public int? MaNguoiDung { get; set; }
        public decimal? TongTien { get; set; }


        public virtual TaiKhoan? MaNguoiDungNavigation { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
