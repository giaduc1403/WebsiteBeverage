using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class LoaiHang
    {
        public LoaiHang()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaLoai { get; set; }
        public string? TenLoai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
