﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppView.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DichVusController : Controller
    {
        private readonly HotelDbContext _context;

        public DichVusController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/DichVu
        public async Task<IActionResult> Index()
        {
            var dichVus = await _context.DichVus.ToListAsync();
            return View(dichVus);
        }

        // GET: DichVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // GET: DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create
        [HttpPost]


        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,Ten,MoTa,Hinh,LoaiPhongId,TrangThai")] DichVu dichVu, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra nếu có hình ảnh được tải lên
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Đường dẫn lưu tệp
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/icon", imageFile.FileName);

                    // Lưu tệp vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Cập nhật đường dẫn hình ảnh vào thuộc tính Hinh
                    dichVu.Hinh = imageFile.FileName;
                }

                _context.Add(dichVu);  // Thêm đối tượng DichVu vào CSDL
                await _context.SaveChangesAsync();  // Lưu thay đổi vào CSDL
                return RedirectToAction(nameof(Index));  // Điều hướng về trang danh sách
            }

            return View(dichVu);  // Nếu có lỗi, trả về lại form với thông báo lỗi
        }



        // GET: DichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: DichVu/Edit/5
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

            // Kiểm tra nếu Model không hợp lệ
          /*  if (ModelState.IsValid)*/
            {
                // Kiểm tra xem người dùng có tải lên tệp ảnh mới không
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu file vào thư mục trên server
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/slider");
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

                    // Cập nhật đường dẫn của hình ảnh vào đối tượng Slide
                    gG.Hinh = imageFile.FileName;
                }
                else
                {
                    // Nếu không có ảnh mới, giữ lại ảnh cũ
                    gG.Hinh = existingSlide.Hinh;
                }

                try
                {
                    _context.Update(gG);
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

            return View(gG);
        }
        private bool SlideExists(int id)
        {
            return _context.Slides.Any(e => e.ID == id);
        }




        // GET: DichVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // POST: DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            _context.DichVus.Remove(dichVu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.ID == id);
        }
    }
}