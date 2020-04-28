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
using MultCo_ISD_API.V1.ControllerContexts;

namespace MultCo_ISD_API.V1.Controllers
{
#if AUTH
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private const string DefaultConnectionStringName = "DefaultConnection";

        private readonly InternalServicesDirectoryV1Context _context;
        private readonly IServiceContextManager _serviceContextManager;

        public ServicesController(InternalServicesDirectoryV1Context context)
        {
            // TODO: Once all CRUD methods use '_serviceContext', remove '_context' as a data member 
            // and pass 'context' directly to the 'ServiceContext' constructor
            _context = context;
            _serviceContextManager = new ServiceContextManager(_context);
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

            //var list = items.Select(i => i.ToServiceV1DTO());
            //return Ok(list);
            var serviceDTOs = new List<ServiceV1DTO>();
            foreach (var s in items)
            {
                serviceDTOs.Add(await FillInService(s).ConfigureAwait(false));
            }
            return Ok(serviceDTOs);
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
            //var item = await _context.Service
            //    .FirstOrDefaultAsync(s => s.ServiceId == id)
            //    .ConfigureAwait(false);

            //if (item == null)
            //{
            //    return NotFound(string.Format("No service found with id = {0}", id));
            //}

            //var itemDTO = await FillInService(item).ConfigureAwait(false);
            //return Ok(itemDTO);

            try
            {
                var service = await _serviceContextManager.GetByIdAsync(id);

                if (service == null)
                {
                    return NotFound();
                }

                return Ok(service.ToServiceV1DTO());
            }
            catch (Exception e)
            {
                throw e;
            }
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

        private async Task<ServiceV1DTO> FillInService(Service service)
        {
            service.Contact = await _context.Contact
                .FirstOrDefaultAsync(c => c.ContactId == service.ContactId)
                .ConfigureAwait(false);

            service.Department = await _context.Department
                .FirstOrDefaultAsync(c => c.DepartmentId == service.DepartmentId)
                .ConfigureAwait(false);

            service.Division = await _context.Division
                .FirstOrDefaultAsync(c => c.DivisionId == service.DivisionId)
                .ConfigureAwait(false);

            service.Program = await _context.Program
                .FirstOrDefaultAsync(c => c.ProgramId == service.ProgramId)
                .ConfigureAwait(false);

            var ServiceCommunityAssociationList = await _context.ServiceCommunityAssociation
                .Where(p => p.ServiceId == service.ServiceId)
                .ToListAsync()
                .ConfigureAwait(false);

            var serviceLanguageAssociationList = await _context.ServiceLanguageAssociation
                .Where(p => p.ServiceId == service.ServiceId)
                .ToListAsync()
                .ConfigureAwait(false);

            var serviceLocationAssociationList = await _context.ServiceLocationAssociation
                .Where(p => p.ServiceId == service.ServiceId)
                .ToListAsync()
                .ConfigureAwait(false);

            var itemDTO = service.ToServiceV1DTO();

            //foreach (var pca in ServiceCommunityAssociationList)
            //{
            //    var community = await _context.Community
            //        .FirstOrDefaultAsync(c => c.CommunityId == pca.CommunityId)
            //        .ConfigureAwait(false);
            //    itemDTO.CommunityDTOs.Add(community.ToCommunityV1DTO());
            //}

            //foreach (var sla in serviceLanguageAssociationList)
            //{
            //    var language = await _context.Language
            //        .FirstOrDefaultAsync(l => l.LanguageId == sla.LanguageId)
            //        .ConfigureAwait(false);
            //    itemDTO.LanguageDTOs.Add(language.ToLanguageV1DTO());
            //}

            //foreach (var sla in serviceLocationAssociationList)
            //{
            //    var location = await _context.Location
            //        .FirstOrDefaultAsync(l => l.LocationId == sla.LocationId)
            //        .ConfigureAwait(false);

            //    location.LocationType = await _context.LocationType
            //        .FirstOrDefaultAsync(lt => lt.LocationTypeId == location.LocationTypeId)
            //        .ConfigureAwait(false);

            //    itemDTO.LocationDTOs.Add(location.ToLocationV1DTO());
            //}

            return itemDTO;
        }
    }
}
