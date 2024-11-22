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
    public class TikToksController : Controller
    {
        private readonly HotelDbContext _context;

        public TikToksController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: TikToks
        public async Task<IActionResult> Index()
        {
            return View(await _context.tikTok.ToListAsync());
        }

        // GET: TikToks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tikTok = await _context.tikTok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tikTok == null)
            {
                return NotFound();
            }

            return View(tikTok);
        }

        // GET: TikToks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TikToks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hinh,Link,TrangThai")] TikTok tikTok)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tikTok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tikTok);
        }

        // GET: TikToks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tikTok = await _context.tikTok.FindAsync(id);
            if (tikTok == null)
            {
                return NotFound();
            }
            return View(tikTok);
        }

        // POST: TikToks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hinh,Link,TrangThai")] TikTok tikTok)
        {
            if (id != tikTok.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tikTok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TikTokExists(tikTok.Id))
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
            return View(tikTok);
        }

        // GET: TikToks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tikTok = await _context.tikTok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tikTok == null)
            {
                return NotFound();
            }

            return View(tikTok);
        }

        // POST: TikToks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tikTok = await _context.tikTok.FindAsync(id);
            if (tikTok != null)
            {
                _context.tikTok.Remove(tikTok);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TikTokExists(int id)
        {
            return _context.tikTok.Any(e => e.Id == id);
        }
    }
}
