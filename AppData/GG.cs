using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData
{
    public class GG
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hình không thể để trống.")]
        public string Hinh { get; set; }

        [Required(ErrorMessage = "Link không thể để trống.")]
        [Url(ErrorMessage = "Link phải là một URL hợp lệ.")]
        public string Link { get; set; }
        public bool TrangThai { get; set; }
    }
}
