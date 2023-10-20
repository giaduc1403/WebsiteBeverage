using WebsiteBeverageDemo.Models;

namespace WebsiteBeverageDemo.Repository
{
    public class LoaiHangRepository : ILoaiHangRepository
    {
        private readonly CHBHTHContext _context;
        public LoaiHangRepository(CHBHTHContext context)
        {
            _context = context;
        }
        public LoaiHang Add(LoaiHang loaiHang)
        {
            _context.LoaiHangs.Add(loaiHang);
            _context.SaveChanges();
            return (loaiHang);
        }

        public LoaiHang Delete(string maloaiHang)
        {
            throw new NotImplementedException();
        }

        public LoaiHang Get(string maloaiHang)
        {
            return _context.LoaiHangs.Find(maloaiHang);
        }

        public IEnumerable<LoaiHang> GetAllLoaiHang()
        {
            return _context.LoaiHangs;
        }

        public LoaiHang Update(LoaiHang loaiHang)
        {
            _context.Update(loaiHang);
            _context.SaveChanges();
            return (loaiHang);
        }
    }
}
