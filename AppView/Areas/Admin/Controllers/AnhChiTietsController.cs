using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppView.ViewModels;

namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnhChiTietsController : Controller
    {
        private readonly HotelDbContext _context;

        public AnhChiTietsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: AnhChiTiets
        public async Task<IActionResult> Index()
        {
            var hotelDbContext = _context.AnhChiTiets.Include(a => a.LoaiPhong);
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: AnhChiTiets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets
                .Include(a => a.LoaiPhong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            return View(anhChiTiet);
        }

        // GET: AnhChiTiets/Create
        // GET: Create
        public IActionResult Create()
        {
            // Lấy danh sách loại phòng để hiển thị trong dropdown
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnhChiTiet anhChiTiet, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có hình ảnh được tải lên
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Đường dẫn lưu tệp
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room-slider", imageFile.FileName);

                    // Kiểm tra và tạo thư mục nếu chưa có
                    var directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Lưu file vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Cập nhật đường dẫn ảnh vào đối tượng
                    anhChiTiet.Anh = imageFile.FileName;
                }

                // Thêm đối tượng AnhChiTiet vào CSDL
                _context.Add(anhChiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, lấy lại danh sách loại phòng
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
            return View(anhChiTiet);
        }


        // GET: AnhChiTiets/Edit/5
        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            // Lấy danh sách loại phòng để hiển thị trong dropdown
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
            return View(anhChiTiet);
        }
        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnhChiTiet anhChiTiet, IFormFile? imageFile)
        {
            if (id != anhChiTiet.Id)
            {
                return NotFound();
            }

            var existingAnhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (existingAnhChiTiet == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra nếu có ảnh mới được tải lên
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room-slider");
                        var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Lưu file vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Cập nhật ảnh mới vào thuộc tính Anh
                        anhChiTiet.Anh = imageFile.FileName;
                    }
                    else
                    {
                        // Nếu không có ảnh mới, giữ lại ảnh cũ
                        anhChiTiet.Anh = existingAnhChiTiet.Anh;
                    }

                    // Cập nhật đối tượng AnhChiTiet trong CSDL
                    _context.Entry(existingAnhChiTiet).CurrentValues.SetValues(anhChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnhChiTietExists(anhChiTiet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Điều hướng về trang danh sách sau khi cập nhật
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, load lại danh sách loại phòng để chọn
            ViewData["IdLoaiPhong"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", anhChiTiet.IdLoaiPhong);
            return View(anhChiTiet);
        }




        // Kiểm tra tồn tại của AnhChiTiet
        private bool AnhChiTietExists(int id)
        {
            return _context.AnhChiTiets.Any(e => e.Id == id);
        }


        // GET: AnhChiTiets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anhChiTiet = await _context.AnhChiTiets
                .Include(a => a.LoaiPhong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anhChiTiet == null)
            {
                return NotFound();
            }

            return View(anhChiTiet);
        }

        // POST: AnhChiTiets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anhChiTiet = await _context.AnhChiTiets.FindAsync(id);
            if (anhChiTiet != null)
            {
                _context.AnhChiTiets.Remove(anhChiTiet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
