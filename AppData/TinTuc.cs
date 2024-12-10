using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class TinTuc
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên tin tức chính không được để trống.")]
        [StringLength(200, ErrorMessage = "Tên tin tức chính không được vượt quá 200 ký tự.")]
        public string? TenTinTucChinh { get; set; }

        [StringLength(200, ErrorMessage = "Tên tin tức phụ không được vượt quá 200 ký tự.")]
        public string? TenTinTucPhu { get; set; }

        [Required(ErrorMessage = "Hình ảnh 1 không được để trống.")]
        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh 1 không được vượt quá 255 ký tự.")]
        public string? HinhAnh1 { get; set; }
        [Required(ErrorMessage = "Hình ảnh 2 không được để trống.")]

        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh 2 không được vượt quá 255 ký tự.")]
        public string? HinhAnh2 { get; set; }
        [Required(ErrorMessage = "Hình ảnh 3 không được để trống.")]

        [StringLength(255, ErrorMessage = "Đường dẫn hình ảnh 3 không được vượt quá 255 ký tự.")]
        public string? HinhAnh3 { get; set; }
        [Required(ErrorMessage = "Nội dung ngắn không được để trống.")]
        [StringLength(500, ErrorMessage = "Nội dung ngắn không được vượt quá 500 ký tự.")]
        public string? NoiDungNgan { get; set; }

        [Required(ErrorMessage = "Nội dung chi tiết 1 không được để trống.")]
        [StringLength(5000, ErrorMessage = "Nội dung chi tiết 1 không được vượt quá 5000 ký tự.")]
        public string NoiDungChiTiet1 { get; set; }

        [Required(ErrorMessage = "Nội dung chi tiết 2 không được để trống.")]
        [StringLength(5000, ErrorMessage = "Nội dung chi tiết 2 không được vượt quá 5000 ký tự.")]
        public string NoiDungChiTiet2 { get; set; }

        [Required(ErrorMessage = "Nội dung chi tiết 3 không được để trống.")]
        [StringLength(5000, ErrorMessage = "Nội dung chi tiết 3 không được vượt quá 5000 ký tự.")]
        public string NoiDungChiTiet3 { get; set; }

        [Required(ErrorMessage = "Nội dung chi tiết 4 không được để trống.")]
        [StringLength(5000, ErrorMessage = "Nội dung chi tiết 4 không được vượt quá 5000 ký tự.")]
        public string NoiDungChiTiet4 { get; set; }
        public bool TrangThai { get; set; }

    }
}
