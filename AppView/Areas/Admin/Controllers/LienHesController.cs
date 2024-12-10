using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace AppView.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class LienHesController : Controller
    {
        private readonly HotelDbContext _context;

        public LienHesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: LienHes
        public async Task<IActionResult> Index()
        {
            return View(await _context.lienHes.ToListAsync());
        }

        // GET: LienHes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.lienHes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }

        // GET: LienHes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LienHes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SoDienThoai,LogoSDT,TrangThai")] LienHe lienHe , IFormFile imageFile)
        {
           /* if (!ModelState.IsValid)*/
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn file vào database
                    lienHe.Logo = imageFile.FileName;
                }
                _context.Add(lienHe);  // Thêm đối tượng DichVu vào CSDL
                _context.SaveChanges();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách

            }
            return View(lienHe);
        }

        // GET: LienHes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.lienHes.FindAsync(id);
            if (lienHe == null)
            {
                return NotFound();
            }
            return View(lienHe);
        }

        // POST: LienHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SoDienThoai,LogoSDT,TrangThai")] LienHe lienHe, IFormFile imageFile)
        {
            if (id != lienHe.ID)
            {
                return NotFound();
            }

            // Tìm đối tượng LienHe hiện tại trong cơ sở dữ liệu
            var existingLienHe = await _context.lienHes.FindAsync(id);
            if (existingLienHe == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu người dùng đã chọn ảnh mới
            if (imageFile != null && imageFile.Length > 0)
            {
                // Đường dẫn thư mục lưu ảnh
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon");
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

                // Cập nhật đường dẫn hình ảnh vào thuộc tính LogoSDT
                lienHe.Logo =  imageFile.FileName;
            }
            else
            {
                // Nếu không chọn ảnh mới, giữ lại hình ảnh cũ
                lienHe.Logo = existingLienHe.Logo;
            }

            try
            {
                // Cập nhật thông tin LienHe vào cơ sở dữ liệu
                _context.Entry(existingLienHe).CurrentValues.SetValues(lienHe);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.lienHes.Any(e => e.ID == lienHe.ID))
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


        // GET: LienHes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lienHe = await _context.lienHes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (lienHe == null)
            {
                return NotFound();
            }

            return View(lienHe);
        }

        // POST: LienHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lienHe = await _context.lienHes.FindAsync(id);
            if (lienHe != null)
            {
                _context.lienHes.Remove(lienHe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LienHeExists(int id)
        {
            return _context.lienHes.Any(e => e.ID == id);
        }
    }
}
