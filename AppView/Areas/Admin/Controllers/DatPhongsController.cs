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
    public class DatPhongsController : Controller
    {
        private readonly HotelDbContext _context;

        public DatPhongsController(HotelDbContext context)
        {
            _context = context;

        }

        // GET: DatPhongs
        public async Task<IActionResult> Index()
        {
            var hotelDbContext = _context.DatPhongs.Include(d => d.LoaiPhong);
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: DatPhongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs
                .Include(d => d.LoaiPhong)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (datPhong == null)
            {
                return NotFound();
            }

            return View(datPhong);
        }

        // GET: DatPhongs/Create





        // GET: DatPhongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong == null)
            {
                return NotFound();
            }
            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LoaiPhongID,KhachHang,CCCD,SoDienThoai,SoNguoiLon,SoTreEm,NgayNhan,NgayTra,SoLuongPhong,GhiChu")] DatPhong datPhong)
        {
            if (id != datPhong.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy dữ liệu cũ từ database để giữ lại NgayDat
                    var originalDatPhong = await _context.DatPhongs.FindAsync(id);
                    if (originalDatPhong == null)
                    {
                        return NotFound();
                    }

                    // Giữ nguyên NgayDat
                    datPhong.NgayDat = originalDatPhong.NgayDat;

                    // Cập nhật đối tượng
                    _context.Entry(originalDatPhong).CurrentValues.SetValues(datPhong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatPhongExists(datPhong.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(datPhong);
            }

            ViewData["LoaiPhongID"] = new SelectList(_context.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong", datPhong.LoaiPhongID);
            return View(datPhong);
        }


        // GET: DatPhongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datPhong = await _context.DatPhongs
                .Include(d => d.LoaiPhong)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (datPhong == null)
            {
                return NotFound();
            }

            return View(datPhong);
        }

        // POST: DatPhongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datPhong = await _context.DatPhongs.FindAsync(id);
            if (datPhong != null)
            {
                _context.DatPhongs.Remove(datPhong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatPhongExists(int id)
        {
            return _context.DatPhongs.Any(e => e.ID == id);
        }
    }
}
