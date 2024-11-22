using AppData;

namespace AppView.ViewModels
{
    public class LoaiPhongVM
    {
        public int MaLoaiPhong { get; set; }
        public string TenLoaiPhong { get; set; }
        public string MoTa { get; set; }
        public string Anh { get; set; }
        public decimal GiaGoc { get; set; }
        public decimal? GiaGiamGia { get; set; }
        public List<DichVuVM> DichVus { get; set; } = new List<DichVuVM>();
    }
}
