using AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.Controllers
{
    public class ViewTinController : Controller
    {

        private readonly HotelDbContext _context;

        public ViewTinController(HotelDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> MyView()
        {
            return View(await _context.tinTucs.ToListAsync());

        }
        public async Task<IActionResult> XemThem(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinTuc = await _context.tinTucs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tinTuc == null)
            {
                return NotFound();
            }

            return View(tinTuc);
        }
    }
}
