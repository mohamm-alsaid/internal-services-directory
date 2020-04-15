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
    public class ServiceLocationAssociationsController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServiceLocationAssociationsController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/ServiceLocationAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceLocationAssociation>>> GetServiceLocationAssociation()
        {
            return await _context.ServiceLocationAssociation.ToListAsync();
        }

        // GET: api/ServiceLocationAssociations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceLocationAssociation>> GetServiceLocationAssociation(int id)
        {
            var serviceLocationAssociation = await _context.ServiceLocationAssociation.FindAsync(id);

            if (serviceLocationAssociation == null)
            {
                return NotFound();
            }

            return serviceLocationAssociation;
        }

        // PUT: api/ServiceLocationAssociations/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceLocationAssociation(int id, ServiceLocationAssociation serviceLocationAssociation)
        {
            if (id != serviceLocationAssociation.ServiceLocationAssociation1)
            {
                return BadRequest();
            }

            _context.Entry(serviceLocationAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLocationAssociationExists(id))
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

        // POST: api/ServiceLocationAssociations
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ServiceLocationAssociation>> PostServiceLocationAssociation(ServiceLocationAssociation serviceLocationAssociation)
        {
            _context.ServiceLocationAssociation.Add(serviceLocationAssociation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceLocationAssociation", new { id = serviceLocationAssociation.ServiceLocationAssociation1 }, serviceLocationAssociation);
        }

        // DELETE: api/ServiceLocationAssociations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceLocationAssociation>> DeleteServiceLocationAssociation(int id)
        {
            var serviceLocationAssociation = await _context.ServiceLocationAssociation.FindAsync(id);
            if (serviceLocationAssociation == null)
            {
                return NotFound();
            }

            _context.ServiceLocationAssociation.Remove(serviceLocationAssociation);
            await _context.SaveChangesAsync();

            return serviceLocationAssociation;
        }

        private bool ServiceLocationAssociationExists(int id)
        {
            return _context.ServiceLocationAssociation.Any(e => e.ServiceLocationAssociation1 == id);
        }
    }
}
