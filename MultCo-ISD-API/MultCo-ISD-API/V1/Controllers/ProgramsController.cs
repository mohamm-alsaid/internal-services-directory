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
    public class ProgramsController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ProgramsController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/Programs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Program>>> GetProgram()
        {
            return await _context.Program.ToListAsync();
        }

        // GET: api/Programs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Program>> GetProgram(int id)
        {
            var program = await _context.Program.FindAsync(id);

            if (program == null)
            {
                return NotFound();
            }

            return program;
        }

        // PUT: api/Programs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgram(int id, Program program)
        {
            if (id != program.ProgramId)
            {
                return BadRequest();
            }

            _context.Entry(program).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramExists(id))
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

        // POST: api/Programs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Program>> PostProgram(Program program)
        {
            _context.Program.Add(program);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProgram", new { id = program.ProgramId }, program);
        }

        // DELETE: api/Programs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Program>> DeleteProgram(int id)
        {
            var program = await _context.Program.FindAsync(id);
            if (program == null)
            {
                return NotFound();
            }

            _context.Program.Remove(program);
            await _context.SaveChangesAsync();

            return program;
        }

        private bool ProgramExists(int id)
        {
            return _context.Program.Any(e => e.ProgramId == id);
        }
    }
}
