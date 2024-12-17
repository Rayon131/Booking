using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace AppView.Controllers
{
	public class LoaiPhongDichVusController : Controller
	{
		private readonly HotelDbContext _context;

		public LoaiPhongDichVusController(HotelDbContext context)
		{
			_context = context;
		}

		// GET: LoaiPhongDichVus
		public async Task<IActionResult> Index()
		{
			var loaiPhongDichVus = await _context.LoaiPhongDichVu
												  .Include(lp => lp.DichVu)
												  .Include(lp => lp.LoaiPhong)
												  .ToListAsync();
			return View(loaiPhongDichVus);
		}

		// GET: LoaiPhongDichVus/Create
		public IActionResult Create()
		{
			ViewData["DichVus"] = new SelectList(_context.DichVus, "ID", "Name");
			ViewData["LoaiPhongs"] = new SelectList(_context.LoaiPhongs, "ID", "Name");
			return View();
		}

		// POST: LoaiPhongDichVus/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("DichVusID,LoaiPhongsId")] LoaiPhongDichVu loaiPhongDichVu)
		{
			if (ModelState.IsValid)
			{
				_context.Add(loaiPhongDichVu);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["DichVus"] = new SelectList(_context.DichVus, "ID", "Name", loaiPhongDichVu.DichVusID);
			ViewData["LoaiPhongs"] = new SelectList(_context.LoaiPhongs, "ID", "Name", loaiPhongDichVu.LoaiPhongsId);
			return View(loaiPhongDichVu);
		}

		// GET: LoaiPhongDichVus/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var loaiPhongDichVu = await _context.LoaiPhongDichVu.FindAsync(id);
			if (loaiPhongDichVu == null)
			{
				return NotFound();
			}
			ViewData["DichVus"] = new SelectList(_context.DichVus, "ID", "Name", loaiPhongDichVu.DichVusID);
			ViewData["LoaiPhongs"] = new SelectList(_context.LoaiPhongs, "ID", "Name", loaiPhongDichVu.LoaiPhongsId);
			return View(loaiPhongDichVu);
		}

		// POST: LoaiPhongDichVus/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,DichVusID,LoaiPhongsId")] LoaiPhongDichVu loaiPhongDichVu)
		{
			if (id != loaiPhongDichVu.ID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(loaiPhongDichVu);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LoaiPhongDichVuExists(loaiPhongDichVu.ID))
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
			ViewData["DichVus"] = new SelectList(_context.DichVus, "ID", "Name", loaiPhongDichVu.DichVusID);
			ViewData["LoaiPhongs"] = new SelectList(_context.LoaiPhongs, "ID", "Name", loaiPhongDichVu.LoaiPhongsId);
			return View(loaiPhongDichVu);
		}

		// GET: LoaiPhongDichVus/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var loaiPhongDichVu = await _context.LoaiPhongDichVu
												 .Include(lp => lp.DichVu)
												 .Include(lp => lp.LoaiPhong)
												 .FirstOrDefaultAsync(lp => lp.ID == id);
			if (loaiPhongDichVu == null)
			{
				return NotFound();
			}

			return View(loaiPhongDichVu);
		}

		// POST: LoaiPhongDichVus/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var loaiPhongDichVu = await _context.LoaiPhongDichVu.FindAsync(id);
			_context.LoaiPhongDichVu.Remove(loaiPhongDichVu);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool LoaiPhongDichVuExists(int id)
		{
			return _context.LoaiPhongDichVu.Any(e => e.ID == id);
		}
	}

}
