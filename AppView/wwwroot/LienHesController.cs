using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace AppView.wwwroot
{
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
        public async Task<IActionResult> Create([Bind("ID,SoDienThoai,LogoSDT,FB,Instagram,TikTok,LogoFB,logoInstagram,LogoTikTok,UrlFb,UrlTikTok,UrlInstagram")] LienHe lienHe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lienHe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,SoDienThoai,LogoSDT,FB,Instagram,TikTok,LogoFB,logoInstagram,LogoTikTok,UrlFb,UrlTikTok,UrlInstagram")] LienHe lienHe)
        {
            if (id != lienHe.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lienHe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LienHeExists(lienHe.ID))
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
            return View(lienHe);
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
