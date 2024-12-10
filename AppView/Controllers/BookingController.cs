using AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelDbContext _context;

        public BookingController(HotelDbContext context)
        {
            _context = context;

        }
        public IActionResult Create()
        {
            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LoaiPhongID,KhachHang,CCCD,SoDienThoai,SoNguoiLon,SoTreEm,NgayNhan,NgayTra,SoLuongPhong,GhiChu")] DatPhong datPhong)
        {
            // Kiểm tra xem LoaiPhongID, SoLuongPhong có giá trị không (null hoặc mặc định DateTime)
            if (datPhong.LoaiPhongID == null || datPhong.SoLuongPhong == null ||
                datPhong.NgayNhan == DateTime.MinValue || datPhong.NgayTra == DateTime.MinValue)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ các thông tin bắt buộc.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong); // Trả lại trang với thông báo lỗi
            }

            // Kiểm tra xem phòng có đủ không
            bool phongTrong = DatPhong.KiemTraPhongTrong(datPhong.LoaiPhongID.Value, datPhong.NgayNhan, datPhong.NgayTra, datPhong.SoLuongPhong.Value, _context);

            if (!phongTrong)
            {
                // Nếu không đủ phòng, thêm lỗi vào ModelState và trả về view hiện tại
                ModelState.AddModelError("", "Không đủ phòng trong khoảng thời gian bạn chọn.");
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
                return View(datPhong); // Trả lại trang với thông báo lỗi
            }

            // Kiểm tra nếu Model hợp lệ
            if (ModelState.IsValid)
            {
                // Lấy giá phòng từ bảng LoaiPhong
                var loaiPhong = await _context.LoaiPhongs
                                                .FirstOrDefaultAsync(lp => lp.MaLoaiPhong == datPhong.LoaiPhongID);

                // Cập nhật thông tin ngày đặt phòng
                datPhong.NgayDat = DateTime.Now;


                // Lưu thông tin vào cơ sở dữ liệu
                _context.Add(datPhong);
                await _context.SaveChangesAsync();

                // Thêm thông báo thành công vào TempData
                TempData["SuccessMessage"] = "Đặt phòng thành công!";
                ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);

                // Chuyển hướng đến trang danh sách hoặc trang chi tiết đặt phòng
                return View(datPhong);
            }

            // Nếu có lỗi, bạn vẫn phải lấy danh sách loại phòng và hiển thị lại form
            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }


    }
}
