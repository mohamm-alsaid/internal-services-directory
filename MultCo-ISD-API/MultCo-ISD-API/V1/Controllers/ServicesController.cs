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
            try
            {
                var services = await _serviceContextManager.GetAllServices();
                if (services == null || services.Count == 0)
                {
                    return NotFound("There are no services in the system!");
                }

                var serviceDTOs = new List<ServiceV1DTO>();
                foreach (var service in services)
                {
                    serviceDTOs.Add(await populateService(service));
                }
                return Ok(serviceDTOs);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            try
            {
                var service = await _serviceContextManager.GetServiceByIdAsync(id);

                if (service == null)
                {
                    return NotFound();
                }

                return Ok(await populateService(service));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //GET: api/Services/Community?="community"
        [Route("[action]/{community}")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
        public async Task<IActionResult> Community(string community)
        {
            try
            {
                //this can be changed to check if the community name contains the input, for now im going for an explicit match of them lowercased
                var comm = await _context.Community
                    .FirstOrDefaultAsync(c => c.CommunityName.ToLower() == community.ToLower())
                    .ConfigureAwait(false);

                if (comm == null)
                {
                    return NotFound("Community given does not exist.");
                }

                //fetch any ServiceCommunityAssociations that have our community's id, then grab the service ids to prep the next DB call to get only the services we want
                var allScas = await _context.ServiceCommunityAssociation
                    .Where(sca => sca.CommunityId == comm.CommunityId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (allScas.Count() == 0)
                {
                    return NotFound("Community has no relationships.");
                }

                var ids = new List<int?>(); //nullable for now, schema has these ids nullable at the moment, will probably fix this in next sprint
                foreach (var sca in allScas)
                {
                    ids.Add(sca.ServiceId);
                }

                //fetch only the services with the service ids we just got from the SCAs, then convert to DTO
                var services = await _context.Service
                    .Where(s => ids.Contains(s.ServiceId))
                    .ToListAsync()
                    .ConfigureAwait(false);

                var serviceDTOs = new List<ServiceV1DTO>();
                foreach (var service in services)
                {
                    serviceDTOs.Add(await populateService(service));
                }
                
                return Ok(serviceDTOs);
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

        private async Task<ServiceV1DTO> populateService(Service service)
        {
            var serviceDTO = service.ToServiceV1DTO();

            foreach (var sca in serviceDTO.ServiceCommunityAssociationDTOs)
            {
                int id;

                if (sca.CommunityID != null)
                {
                    id = (int)sca.CommunityID;

                    var comm = await _serviceContextManager.GetCommunityByIdAsync(id);

                    if (comm == null)
                    {
                        continue;
                    }

                    serviceDTO.CommunityDTOs.Add(comm.ToCommunityV1DTO());
                }
            }

            foreach (var sla in serviceDTO.ServiceLanguageAssociationDTOs)
            {
                int id;
                if (sla.LanguageID != null)
                {
                    id = (int)sla.LanguageID;
                    var lang = await _serviceContextManager.GetLanguageByIdAsync(id);
                    if (lang == null)
                    {
                        continue;
                    }
                    serviceDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());
                }
            }

            foreach (var sla in serviceDTO.ServiceLocationAssociationDTOs)
            {
                int id;
                if (sla.LocationID != null)
                {
                    id = (int)sla.LocationID;
                    var loc = await _serviceContextManager.GetLocationByIdAsync(id);
                    if (loc == null)
                    {
                        continue;
                    }
                    serviceDTO.LocationDTOs.Add(loc.ToLocationV1DTO());
                }
            }
            return serviceDTO;
        }
    }
}
