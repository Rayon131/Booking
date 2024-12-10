using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class LienHe
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^(\+84|0)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Hình không thể để trống.")]
        public string? LogoSDT { get; set; }
        public bool TrangThai { get; set; }

    }
}
