using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongChiTietsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public PhongChiTietsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/PhongChiTiets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhongChiTiet>>> GetPhongChiTiets()
        {
            return await _context.PhongChiTiets.ToListAsync();
        }

        // GET: api/PhongChiTiets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhongChiTiet>> GetPhongChiTiet(int id)
        {
            var phongChiTiet = await _context.PhongChiTiets.FindAsync(id);

            if (phongChiTiet == null)
            {
                return NotFound();
            }

            return phongChiTiet;
        }

        // PUT: api/PhongChiTiets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhongChiTiet(int id, PhongChiTiet phongChiTiet)
        {
            if (id != phongChiTiet.ID)
            {
                return BadRequest();
            }

            _context.Entry(phongChiTiet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhongChiTietExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PhongChiTiets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhongChiTiet>> PostPhongChiTiet(PhongChiTiet phongChiTiet)
        {
            _context.PhongChiTiets.Add(phongChiTiet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhongChiTiet", new { id = phongChiTiet.ID }, phongChiTiet);
        }

        // DELETE: api/PhongChiTiets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhongChiTiet(int id)
        {
            var phongChiTiet = await _context.PhongChiTiets.FindAsync(id);
            if (phongChiTiet == null)
            {
                return NotFound();
            }

            _context.PhongChiTiets.Remove(phongChiTiet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhongChiTietExists(int id)
        {
            return _context.PhongChiTiets.Any(e => e.ID == id);
        }
    }
}
