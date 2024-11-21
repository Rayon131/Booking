using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class DatPhong
	{
		[Key]
		public int ID { get; set; }
		public int? IDPhongChiTiet { get; set; }
		public string KhachHang { get; set; }
		public int CCCD { get; set; }
		public string SoDienThoai { get; set; }
		public int SoNguoiLon {  get; set; }
		public int SoTreEm { get; set; }
		public DateTime NgayDat {  get; set; }
		public DateTime NgayNhan { get; set; }
		public DateTime NgayTra { get; set; }
		public int SoLuongPhong { get; set; }
		public string GhiChu { get; set; }
		public string TrangThai { get; set; }
		public decimal TongTien { get; set; }

		public PhongChiTiet? PhongChiTiet { get; set; }
	}
}
