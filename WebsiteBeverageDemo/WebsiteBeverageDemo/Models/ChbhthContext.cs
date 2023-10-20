using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebsiteBeverageDemo.ModelViews;

namespace WebsiteBeverageDemo.Models
{
    public partial class CHBHTHContext : DbContext
    {
        public CHBHTHContext()
        {
        }

        public CHBHTHContext(DbContextOptions<CHBHTHContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<LoaiHang> LoaiHangs { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<PhanQuyen> PhanQuyens { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
        public virtual DbSet<TinTuc> TinTucs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI\\GIADUC14; Database = CHBHTH; Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.CtmaDon)
                    .HasName("PK__ChiTietD__C5D66E494F56F56D");

                entity.ToTable("ChiTietDonHang");

                entity.Property(e => e.CtmaDon).HasColumnName("CTMaDon");

                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.MaDonNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaDon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cthd_hd");

                entity.HasOne(d => d.MaSpNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cthd_sp");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.Madon)
                    .HasName("PK__DonHang__2D25D8D29D75DA4E");

                entity.ToTable("DonHang");

                entity.Property(e => e.DiaChiNhanHang).HasMaxLength(100);

                entity.Property(e => e.NgayDat).HasColumnType("datetime");

                entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.MaNguoiDungNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaNguoiDung)
                    .HasConstraintName("FK_hd_tk");
            });

            modelBuilder.Entity<LoaiHang>(entity =>
            {
                entity.HasKey(e => e.MaLoai)
                    .HasName("PK__LoaiHang__730A5759C02D4885");

                entity.ToTable("LoaiHang");

                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNcc)
                    .HasName("PK__NhaCungC__3A185DEBC6B6B848");

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.TenNcc)
                    .HasMaxLength(100)
                    .HasColumnName("TenNCC");
            });

            modelBuilder.Entity<PhanQuyen>(entity =>
            {
                entity.HasKey(e => e.Idquyen)
                    .HasName("PK__PhanQuye__B3A2827E6405557E");

                entity.ToTable("PhanQuyen");

                entity.Property(e => e.Idquyen).HasColumnName("IDQuyen");

                entity.Property(e => e.TenQuyen).HasMaxLength(20);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp)
                    .HasName("PK__SanPham__2725081C1DF952CB");

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.AnhSp).HasColumnName("AnhSP");

                entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MaNcc).HasColumnName("MaNCC");

                entity.Property(e => e.MoTa).HasColumnType("ntext");

                entity.Property(e => e.TenSp)
                    .HasMaxLength(100)
                    .HasColumnName("TenSP");

                entity.HasOne(d => d.MaLoaiNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaLoai)
                    .HasConstraintName("FK_sp_loai");

                entity.HasOne(d => d.MaNccNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaNcc)
                    .HasConstraintName("FK_sp_ncc");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung)
                    .HasName("PK__TaiKhoan__C539D762044F6324");

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.Diachi).HasMaxLength(100);

                entity.Property(e => e.Dienthoai)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.Idquyen).HasColumnName("IDQuyen");

                entity.Property(e => e.Matkhau)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdquyenNavigation)
                    .WithMany(p => p.TaiKhoans)
                    .HasForeignKey(d => d.Idquyen)
                    .HasConstraintName("FK_tk_pq");
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.HasKey(e => e.MaTt)
                    .HasName("PK__TinTuc__27250079C821EF13");

                entity.ToTable("TinTuc");

                entity.Property(e => e.MaTt).HasColumnName("MaTT");

                entity.Property(e => e.NoiDung).HasColumnType("ntext");

                entity.Property(e => e.TieuDe).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<WebsiteBeverageDemo.ModelViews.DangKyViewModel>? DangKyViewModel { get; set; }

        public DbSet<WebsiteBeverageDemo.ModelViews.LoginViewModel>? LoginViewModel { get; set; }
    }
}
