using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class PhanQuyen
    {
        public PhanQuyen()
        {
            TaiKhoans = new HashSet<TaiKhoan>();
        }

        public int Idquyen { get; set; }
        public string? TenQuyen { get; set; }

        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
