using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppView.ViewModels;

namespace AppView.Controllers
{
    public class TaiKhoannsController : Controller
    {
        private readonly HotelDbContext _context;

        public TaiKhoannsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoanns
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaiKhoans.ToListAsync());
        }
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
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
            return RedirectToAction("Login", "Account");
        }

        // GET: TaiKhoanns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoann = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taiKhoann == null)
            {
                return NotFound();
            }

            return View(taiKhoann);
        }

        // GET: TaiKhoanns/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TaiKhoan,MatKhau,Quyen")] TaiKhoann taiKhoann)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoann);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoann);
        }

        // GET: TaiKhoanns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoann = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoann == null)
            {
                return NotFound();
            }
            return View(taiKhoann);
        }

        // POST: TaiKhoanns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TaiKhoan,MatKhau")] TaiKhoann taiKhoann)
        {
            if (id != taiKhoann.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoann);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoannExists(taiKhoann.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoann);
        }

        // GET: TaiKhoanns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoann = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.ID == id);
            if (taiKhoann == null)
            {
                return NotFound();
            }

            return View(taiKhoann);
        }

        // POST: TaiKhoanns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taiKhoann = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoann != null)
            {
                _context.TaiKhoans.Remove(taiKhoann);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoannExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.ID == id);
        }
    }
}
