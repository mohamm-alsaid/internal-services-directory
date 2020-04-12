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
    public class ProgramCommunityAssociationsController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ProgramCommunityAssociationsController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/ProgramCommunityAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramCommunityAssociation>>> GetProgramCommunityAssociation()
        {
            return await _context.ProgramCommunityAssociation.ToListAsync();
        }

        // GET: api/ProgramCommunityAssociations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgramCommunityAssociation>> GetProgramCommunityAssociation(int id)
        {
            var programCommunityAssociation = await _context.ProgramCommunityAssociation.FindAsync(id);

            if (programCommunityAssociation == null)
            {
                return NotFound();
            }

            return programCommunityAssociation;
        }

        // PUT: api/ProgramCommunityAssociations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgramCommunityAssociation(int id, ProgramCommunityAssociation programCommunityAssociation)
        {
            if (id != programCommunityAssociation.ProgramCommunityAssociationId)
            {
                return BadRequest();
            }

            _context.Entry(programCommunityAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramCommunityAssociationExists(id))
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

        // POST: api/ProgramCommunityAssociations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProgramCommunityAssociation>> PostProgramCommunityAssociation(ProgramCommunityAssociation programCommunityAssociation)
        {
            _context.ProgramCommunityAssociation.Add(programCommunityAssociation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProgramCommunityAssociation", new { id = programCommunityAssociation.ProgramCommunityAssociationId }, programCommunityAssociation);
        }

        // DELETE: api/ProgramCommunityAssociations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProgramCommunityAssociation>> DeleteProgramCommunityAssociation(int id)
        {
            var programCommunityAssociation = await _context.ProgramCommunityAssociation.FindAsync(id);
            if (programCommunityAssociation == null)
            {
                return NotFound();
            }

            _context.ProgramCommunityAssociation.Remove(programCommunityAssociation);
            await _context.SaveChangesAsync();

            return programCommunityAssociation;
        }

        private bool ProgramCommunityAssociationExists(int id)
        {
            return _context.ProgramCommunityAssociation.Any(e => e.ProgramCommunityAssociationId == id);
        }
    }
}
