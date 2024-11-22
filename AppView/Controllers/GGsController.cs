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
    public class GGsController : Controller
    {
        private readonly HotelDbContext _context;

        public GGsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: GGs
        public async Task<IActionResult> Index()
        {
            return View(await _context.gGs.ToListAsync());
        }

        // GET: GGs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gG = await _context.gGs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gG == null)
            {
                return NotFound();
            }

            return View(gG);
        }

        // GET: GGs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hinh,Link,TrangThai")] GG gG)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gG);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gG);
        }

        // GET: GGs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gG = await _context.gGs.FindAsync(id);
            if (gG == null)
            {
                return NotFound();
            }
            return View(gG);
        }

        // POST: GGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hinh,Link,TrangThai")] GG gG)
        {
            if (id != gG.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GGExists(gG.Id))
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

        // GET: GGs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gG = await _context.gGs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gG == null)
            {
                return NotFound();
            }

            return View(gG);
        }

        // POST: GGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gG = await _context.gGs.FindAsync(id);
            if (gG != null)
            {
                _context.gGs.Remove(gG);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GGExists(int id)
        {
            return _context.gGs.Any(e => e.Id == id);
        }
    }
}
