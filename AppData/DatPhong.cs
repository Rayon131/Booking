using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    using System.ComponentModel.DataAnnotations;

    public class DatPhong
    {
        public DatPhong()
        {
            NgayDat = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        public int? LoaiPhongID { get; set; }

        [Required(ErrorMessage = "Khách hàng không được để trống.")]
        public string? KhachHang { get; set; }
        [Required(ErrorMessage = "Căn cước công dân không được để trống.")]

        [RegularExpression(@"^\d{12}$", ErrorMessage = "Căn cước công dân phải chứa đúng 12 chữ số.")]
        public string? CCCD { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^(\+84|0)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Số người lớn không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số người lớn phải lớn hơn hoặc bằng 1.")]
        public int SoNguoiLon { get; set; }

        [Required(ErrorMessage = "Số trẻ em không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em phải là số không âm.")]
        public int SoTreEm { get; set; }


        [Required(ErrorMessage = "Ngày đặt phòng không được để trống.")]
        public DateTime NgayDat { get; set; }

        [Required(ErrorMessage = "Ngày nhận phòng không được để trống.")]
        [CustomValidation(typeof(DatPhong), nameof(ValidateNgayNhan))]
        public DateTime NgayNhan { get; set; }

        [Required(ErrorMessage = "Ngày trả phòng không được để trống.")]
        [CustomValidation(typeof(DatPhong), nameof(ValidateNgayTra))]
        public DateTime NgayTra { get; set; }
        [Required(ErrorMessage = "Số lượng phòng không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phòng phải lớn hơn 0.")]
        public int? SoLuongPhong { get; set; }

        public string? GhiChu { get; set; }

        public LoaiPhong? LoaiPhong { get; set; }

        public static bool KiemTraPhongTrong(int loaiPhongID, DateTime ngayNhan, DateTime ngayTra, int soLuongPhong, HotelDbContext context)
        {
            // Lấy thông tin loại phòng, bao gồm tổng số phòng
            var loaiPhong = context.LoaiPhongs.FirstOrDefault(lp => lp.MaLoaiPhong == loaiPhongID);

            if (loaiPhong == null)
            {
                return false; // Nếu không tìm thấy loại phòng thì trả về false
            }

            int tongSoPhong = loaiPhong.SoPhongCon;  // Giả sử trường này lưu số phòng tổng của loại phòng

            // Lấy tất cả các đặt phòng đã có cho loại phòng này trong khoảng thời gian
            var danhSachDatPhong = context.DatPhongs
                .Where(d => d.LoaiPhongID == loaiPhongID
                            && ((d.NgayNhan >= ngayNhan && d.NgayNhan <= ngayTra)
                            || (d.NgayTra >= ngayNhan && d.NgayTra <= ngayTra)))
                .ToList();

            // Kiểm tra số lượng phòng đã đặt
            int soPhongDaDat = danhSachDatPhong.Sum(d => d.SoLuongPhong ?? 0);

            // Kiểm tra số lượng phòng còn lại
            if (soPhongDaDat + soLuongPhong > tongSoPhong)
            {
                return false;  // Không đủ phòng
            }

            return true;  // Có phòng
        }
        public void CapNhatLichDatPhong(int loaiPhongId, DateTime ngayNhan, DateTime ngayTra, int soLuongDat, HotelDbContext db)
        {
            for (var ngay = ngayNhan; ngay < ngayTra; ngay = ngay.AddDays(1))
            {
                var lich = db.LichDatPhongs.FirstOrDefault(l => l.LoaiPhongID == loaiPhongId && l.Ngay == ngay);

                if (lich == null)
                {
                    // Nếu chưa có lịch cho ngày này, thêm mới
                    db.LichDatPhongs.Add(new LichDatPhong
                    {
                        LoaiPhongID = loaiPhongId,
                        Ngay = ngay,
                        SoLuongDaDat = soLuongDat
                    });
                }
                else
                {
                    // Nếu đã có lịch, cập nhật số lượng phòng
                    lich.SoLuongDaDat += soLuongDat;
                }
            }

            db.SaveChanges();
        }


        // Custom validation methods
        public static ValidationResult? ValidateNgayNhan(DateTime ngayNhan, ValidationContext context)
        {
            var instance = (DatPhong)context.ObjectInstance;

            if (ngayNhan < instance.NgayDat)
            {
                return new ValidationResult("Ngày nhận không được nhỏ hơn ngày đặt.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateNgayTra(DateTime ngayTra, ValidationContext context)
        {
            var instance = (DatPhong)context.ObjectInstance;

            if (ngayTra < instance.NgayDat)
            {
                return new ValidationResult("Ngày trả không được nhỏ hơn ngày đặt.");
            }

            if (ngayTra < instance.NgayNhan)
            {
                return new ValidationResult("Ngày trả không được nhỏ hơn ngày nhận.");
            }

            return ValidationResult.Success;
        }

    }

}
