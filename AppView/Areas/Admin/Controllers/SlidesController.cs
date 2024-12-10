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
    public class SlidesController : Controller
    {
        private readonly HotelDbContext _context;

        public SlidesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Slides
        public async Task<IActionResult> Index()
        {
            return View(await _context.Slides.ToListAsync());
        }

        // GET: Slides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // GET: Slides/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Create([Bind("Id,NoiDung,Hinh,TrangThai")] Slide gG, IFormFile imageFile)
        {

            if (!ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu file vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider", imageFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn file vào database
                    gG.Hinh = imageFile.FileName;
                }

                _context.Add(gG);  // Thêm đối tượng DichVu vào CSDL
                _context.SaveChanges();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách
            }

            return View(gG);
        }

        // GET: FaceBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gg = await _context.Slides.FindAsync(id);
            if (gg == null)
            {
                return NotFound();
            }
            return View(gg);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("ID,NoiDung,Hinh,TrangThai")] Slide gG, IFormFile imageFile)
        {
            if (id != gG.ID)
            {
                return NotFound();
            }

            var existingSlide = await _context.Slides.FindAsync(id);
            if (existingSlide == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu có ảnh mới được tải lên
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider");
                var filePath = Path.Combine(uploadsFolder, imageFile.FileName);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Lưu ảnh vào thư mục trên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                gG.Hinh = imageFile.FileName;  // Cập nhật ảnh mới
            }
            else
            {
                // Nếu không có ảnh mới, giữ lại ảnh cũ
                gG.Hinh = existingSlide.Hinh;
            }

            try
            {
                // Cập nhật giá trị các trường khác từ đối tượng gG
                _context.Entry(existingSlide).CurrentValues.SetValues(gG);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlideExists(gG.ID))
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




        // GET: Slides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slide = await _context.Slides
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slide == null)
            {
                return NotFound();
            }

            return View(slide);
        }

        // POST: Slides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slide = await _context.Slides.FindAsync(id);
            if (slide != null)
            {
                _context.Slides.Remove(slide);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlideExists(int id)
        {
            return _context.Slides.Any(e => e.ID == id);
        }
    }
}
