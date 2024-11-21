using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
	public class PhongChiTiet
	{
		[Key]
		public int ID { get; set; }
		public string TenPhong { get; set; }
		public int SoNguoi { get; set; }
		public decimal Gia { get; set; }
		public LoaiPhong? LoaiPhong { get; set; }
	}
}
