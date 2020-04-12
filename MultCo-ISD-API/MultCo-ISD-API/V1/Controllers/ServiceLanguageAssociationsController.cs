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
    public class ServiceLanguageAssociationsController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceLanguageAssociationsController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/ServiceLanguageAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceLanguageAssociation>>> GetServiceLanguageAssociation()
        {
            return await _context.ServiceLanguageAssociation.ToListAsync();
        }

        // GET: api/ServiceLanguageAssociations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceLanguageAssociation>> GetServiceLanguageAssociation(int id)
        {
            var serviceLanguageAssociation = await _context.ServiceLanguageAssociation.FindAsync(id);

            if (serviceLanguageAssociation == null)
            {
                return NotFound();
            }

            return serviceLanguageAssociation;
        }

        // PUT: api/ServiceLanguageAssociations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceLanguageAssociation(int id, ServiceLanguageAssociation serviceLanguageAssociation)
        {
            if (id != serviceLanguageAssociation.ServiceLanguageAssociation1)
            {
                return BadRequest();
            }

            _context.Entry(serviceLanguageAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLanguageAssociationExists(id))
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

        // POST: api/ServiceLanguageAssociations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ServiceLanguageAssociation>> PostServiceLanguageAssociation(ServiceLanguageAssociation serviceLanguageAssociation)
        {
            _context.ServiceLanguageAssociation.Add(serviceLanguageAssociation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceLanguageAssociation", new { id = serviceLanguageAssociation.ServiceLanguageAssociation1 }, serviceLanguageAssociation);
        }

        // DELETE: api/ServiceLanguageAssociations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceLanguageAssociation>> DeleteServiceLanguageAssociation(int id)
        {
            var serviceLanguageAssociation = await _context.ServiceLanguageAssociation.FindAsync(id);
            if (serviceLanguageAssociation == null)
            {
                return NotFound();
            }

            _context.ServiceLanguageAssociation.Remove(serviceLanguageAssociation);
            await _context.SaveChangesAsync();

            return serviceLanguageAssociation;
        }

        private bool ServiceLanguageAssociationExists(int id)
        {
            return _context.ServiceLanguageAssociation.Any(e => e.ServiceLanguageAssociation1 == id);
        }
    }
}
