using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class LoaiPhong
	{
		[Key]
		public int MaLoaiPhong { get; set; }
		public string TenLoaiPhong { get; set; }
		public string MoTa {  get; set; }
		public string Anh { get; set; }
		public decimal GiaGoc { get; set; }
		public decimal? GiaGiamGia { get; set; }
		public ICollection<PhongChiTiet>? phongs { get; set; }
        public ICollection<AnhChiTiet>? HinhAnhPhongs { get; set; }
        public ICollection<DichVu>? DichVus { get; set; }
    }
}
