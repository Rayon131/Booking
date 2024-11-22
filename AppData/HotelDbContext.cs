using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class HotelDbContext : DbContext
	{
		public HotelDbContext(DbContextOptions options) : base(options)
		{
		}
		public HotelDbContext() { }
		public DbSet<LoaiPhong> LoaiPhongs { get; set; }
		public DbSet<DichVu> DichVus { get; set; }
		public DbSet<PhongChiTiet> PhongChiTiets { get; set; }
		public DbSet<TaiKhoann> TaiKhoans { get; set; }
		public DbSet<DatPhong> DatPhongs { get; set; }
		public DbSet<AnhChiTiet> AnhChiTiets { get; set; }
		public DbSet<LienHe> lienHes { get; set; }
		public DbSet<LoGo> loGos { get; set; }
		public DbSet<MenuItem> Menu { get; set; }
		public DbSet<Slide> Slides { get; set; }
		public DbSet<TinTuc> tinTucs { get; set; }
		public DbSet<WelCome> welComes { get; set; }
		
	


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LoaiPhong>()
			 .HasMany(lp => lp.DichVus)
			 .WithMany(dv => dv.LoaiPhongs)
			 .UsingEntity(j => j.ToTable("LoaiPhongDichVu"));
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=NOONE\\MSSQLSERVER02;Initial Catalog=BOOKING2;Integrated Security=True;Trust Server Certificate=True");
		}

	}
}

