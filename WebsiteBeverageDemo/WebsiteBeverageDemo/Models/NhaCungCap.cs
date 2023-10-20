using System;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaNcc { get; set; }
        public string? TenNcc { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
