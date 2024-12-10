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
    public class LoGoesController : Controller
    {
        private readonly HotelDbContext _context;

        public LoGoesController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.loGos.ToList());
        }

        // GET: LoGo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoGo/Create
        [HttpPost]

        public async Task<IActionResult> Create([Bind("ID,Logo,TrangThai")] LoGo logo, IFormFile imageFile)
        {

            /*if (!ModelState.IsValid)*/
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn file vào database
                    logo.Logo = imageFile.FileName;
                }

                _context.Add(logo);  // Thêm đối tượng DichVu vào CSDL
                _context.SaveChanges();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách
            }

            return View(logo);
        }

        // GET: LoGoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loGo = await _context.loGos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loGo == null)
            {
                return NotFound();
            }

            return View(loGo);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.loGos.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LoGo logo, IFormFile imageFile)
        {
            if (id != logo.ID)
            {
                return NotFound();
            }

            // Tìm logo hiện tại trong cơ sở dữ liệu
            var existingLogo = await _context.loGos.FindAsync(id);
            if (existingLogo == null)
            {
                return NotFound();
            }

            // Kiểm tra xem người dùng có tải lên tệp mới không
            if (imageFile != null && imageFile.Length > 0)
            {
                // Lưu file mới vào thư mục trên server
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Lưu tệp vào server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn file mới vào logo
                logo.Logo =imageFile.FileName;
            }
            else
            {
                // Nếu không tải lên file mới, giữ lại đường dẫn file cũ
                logo.Logo = existingLogo.Logo;
            }

            try
            {
                // Cập nhật thông tin logo
                _context.Entry(existingLogo).CurrentValues.SetValues(logo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoGoExists(logo.ID))
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

        // Hàm kiểm tra tồn tại
      /*  private bool LoGoExists(int id)
        {
            return _context.LoGoes.Any(e => e.ID == id);
        }*/


        // GET: LoGoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loGo = await _context.loGos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (loGo == null)
            {
                return NotFound();
            }

            return View(loGo);
        }

        // POST: LoGoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loGo = await _context.loGos.FindAsync(id);
            if (loGo != null)
            {
                _context.loGos.Remove(loGo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoGoExists(int id)
        {
            return _context.loGos.Any(e => e.ID == id);
        }
    }
}
