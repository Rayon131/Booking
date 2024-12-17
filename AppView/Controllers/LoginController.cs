using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class LoginController : Controller
    {
        private readonly HotelDbContext _context;

        public LoginController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoanns
      
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm tài khoản trong cơ sở dữ liệu
                var user = await _context.TaiKhoans
                    .Where(t => t.TaiKhoan == model.Username)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    // Kiểm tra mật khẩu
                    if (user.MatKhau == model.Password)
                    {
                        // Đăng nhập thành công, lưu thông tin vào session
                        HttpContext.Session.SetString("TaiKhoan", user.TaiKhoan);
                        HttpContext.Session.SetString("Quyen", user.Quyen);

                        // Kiểm tra quyền và thay đổi layout
                        if (user.Quyen == "admin")
                        {
                            // Layout cho Admin
                            ViewData["Layout"] = "_AdminLayout";
                            return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });
                        }
                        else if (user.Quyen == "Manager")
                        {
                            // Layout cho Manager
                            ViewData["Layout"] = "_ManagerLayout";
                            return RedirectToAction("Index", "ManagerDashboard", new { area = "Manager" });
                        }
                        else
                        {
                            // Layout cho người dùng bình thường
                            ViewData["Layout"] = "_UserLayout";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        // Thông báo lỗi nếu mật khẩu sai
                        ModelState.AddModelError("", "Mật khẩu không chính xác.");
                    }
                }
                else
                {
                    // Thông báo lỗi nếu tài khoản không tồn tại
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
            }

            return View(model);
        }



        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
