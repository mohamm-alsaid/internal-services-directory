using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public DivisionsController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivision()
        {
            return await _context.Division.ToListAsync();
        }

        // GET: api/Divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Division>> GetDivision(int id)
        {
            var division = await _context.Division.FindAsync(id);

            if (division == null)
            {
                return NotFound();
            }

            return division;
        }

        // PUT: api/Divisions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, Division division)
        {
            if (id != division.DivisionId)
            {
                return BadRequest();
            }

            _context.Entry(division).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisionExists(id))
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

        // POST: api/Divisions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Division>> PostDivision(Division division)
        {
            _context.Division.Add(division);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDivision", new { id = division.DivisionId }, division);
        }

        // DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Division>> DeleteDivision(int id)
        {
            var division = await _context.Division.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }

            _context.Division.Remove(division);
            await _context.SaveChangesAsync();

            return division;
        }

        private bool DivisionExists(int id)
        {
            return _context.Division.Any(e => e.DivisionId == id);
        }
    }
}
