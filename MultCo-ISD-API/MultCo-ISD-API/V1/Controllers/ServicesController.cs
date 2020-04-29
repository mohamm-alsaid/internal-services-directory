using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;

namespace MultCo_ISD_API.V1.Controllers
{
#if AUTH
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly InternalServicesDirectoryV1Context _context;

        public ServicesController(InternalServicesDirectoryV1Context context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceV1DTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
        public async Task<IActionResult> GetServices()
        {
            var items = await _context.Service
                .OrderBy(s => s.ServiceName)
                .ToListAsync()
                .ConfigureAwait(false);

            if (items == null || items.Count == 0)
            {
                var message = "There are no services in the system!";
                return NotFound(message);
            }

            var list = items.Select(i => i.ToServiceV1DTO());
            return Ok(list);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
        public async Task<IActionResult> GetService(int id)
        {
            var a = _context;
            var item = await _context.Service
                .FirstOrDefaultAsync(s => s.ServiceId == id)
                .ConfigureAwait(false);

            if (item == null)
            {
                return NotFound(string.Format("No service found with id = {0}", id));
            }

            return Ok(item.ToServiceV1DTO());
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
        public async Task<IActionResult> PutService(int id, [FromBody] ServiceV1DTO serviceV1DTO)
        {
            if (serviceV1DTO == null)
            {
                throw new ArgumentNullException(nameof(serviceV1DTO));
            }

            if (id != serviceV1DTO.ServiceId)
            {
                return BadRequest();
            }

            var item = await _context.Service
                .FirstOrDefaultAsync(s => s.ServiceId == serviceV1DTO.ServiceId)
                .ConfigureAwait(false);

            if (item == null)
            {
                return NotFound();
            }

            item.CopyFromServiceV1DTO(serviceV1DTO);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }

        // POST: api/Services
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
        public async Task<IActionResult> PostService([FromBody] ServiceV1DTO serviceV1DTO)
        {
            if (serviceV1DTO == null)
            {
                throw new ArgumentNullException(nameof(serviceV1DTO));
            }

            var item = await _context.Service
                .FirstOrDefaultAsync(s => s.ServiceId == serviceV1DTO.ServiceId)
                .ConfigureAwait(false);

            if (item != null)
            {
                return Conflict(); // 409
            }

            _context.Service.Add(serviceV1DTO.ToService());

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
        public async Task<IActionResult> DeleteService(int id)
        {
            var item = await _context.Service.FirstOrDefaultAsync(s => s.ServiceId == id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Service.Remove(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return NoContent(); // 204
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ServiceId == id);
        }
    }
}
