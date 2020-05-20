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
using MultCo_ISD_API.Validation;

namespace MultCo_ISD_API.V1.Controllers
{
#if AUTH
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
#endif

	[Validate]
	[Route("services/api/v1/[controller]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private const string DefaultConnectionStringName = "DefaultConnection";

		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IServiceContextManager _serviceContextManager;

		public ServiceController(InternalServicesDirectoryV1Context context)
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
		public async Task<IActionResult> GetServices(int pageSize = 20, int pageIndex = 0)
		{
			try
			{
				if (pageSize < 0 || pageIndex < 0)
				{
					return NotFound("Invalid page index or page size.");
				}
				var services = await _serviceContextManager.GetAllServices(pageSize, pageIndex);
				if (services == null || services.Count == 0)
				{
					return NotFound("No services were found with the given page information.");
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

		//GET: api/Services/lang?="language"
		[HttpGet]
		[Route("[action]/{lang}")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Language(string lang)
		{
			//massage query string into a list
			var langNames = lang.Split(',');
			var langNamesList = new List<string>(langNames);
			var langs = await _serviceContextManager.GetLanguagesByNameListAsync(langNamesList);

			if (langs.Count() == 0)
			{
				return NotFound("No languages from given names found.");
			}

			var langIds = new List<int>();
			foreach (var language in langs)
			{
				langIds.Add(language.LanguageId);
			}

			var slas = await _serviceContextManager.GetServiceLanguageAssociationsByLanguageIdListAsync(langIds);

			if (slas.Count() == 0)
			{
				return NotFound("No relationships found for given language(s).");
			}

			var serviceIds = new List<int>();
			foreach (var sla in slas)
			{
				serviceIds.Add(sla.ServiceId);
			}

			var services = await _serviceContextManager.GetServicesFromIdList(serviceIds);
			var serviceDTOs = new List<ServiceV1DTO>();
			foreach (var service in services)
			{
				serviceDTOs.Add(await populateService(service));
			}

			return Ok(serviceDTOs);
		}

		//GET: api/Services/Community?="community"
		[HttpGet]
		[Route("[action]/{comm}")]
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
				var comm = await _serviceContextManager.GetCommunityByNameAsync(community);

				if (comm == null)
				{
					return NotFound("Community given does not exist.");
				}

				//fetch any ServiceCommunityAssociations that have our community's id, then grab the service ids to prep the next DB call to get only the services we want
				var scas = await _serviceContextManager.GetServiceCommunityAssociationsByCommunityIdAsync(comm.CommunityId);

				if (scas.Count() == 0)
				{
					return NotFound("Community has no relationships.");
				}

				var ids = new List<int>();
				foreach (var sca in scas)
				{
					ids.Add(sca.ServiceId);
				}

				//fetch only the services with the service ids we just got from the SCAs, then convert to DTO
				var services = _serviceContextManager.GetServicesFromIdList(ids).Result;

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

		// GET: api/Service/BuildingId
		[HttpGet]
		[Route("[action]/{buildingId}")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> BuildingId(string buildingId)
		{
			var locations = await _serviceContextManager.GetLocationsByBuildingId(buildingId);
			if (locations.Count() == 0)
			{
				return NotFound("No locations found from given building id.");
			}

			var locationIds = new List<int>();
			foreach (var l in locations)
			{
				locationIds.Add(l.LocationId);
			}

			var slas = await _serviceContextManager.GetServiceLocationAssociationsByLocationIdListAsync(locationIds);
			if (slas.Count() == 0)
			{
				return NotFound("Location(s) found have no relationships to any services.");
			}

			var serviceIds = new List<int>();
			foreach (var sla in slas)
			{
				serviceIds.Add(sla.ServiceId);
			}

			var services = await _serviceContextManager.GetServicesFromIdList(serviceIds);
			var serviceDTOs = new List<ServiceV1DTO>();
			foreach (var service in services)
			{
				serviceDTOs.Add(await populateService(service));
			}

			return Ok(serviceDTOs);
		}

		//GET: api/Services/Program?="programId"
		[HttpGet]
		[Route("[action]/{programId}")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Program(int programId)
		{
			var services = await _serviceContextManager.GetServicesFromProgramId(programId);

			if (services.Count() == 0)
			{
				return NotFound("No services found with given program id.");
			}

			var serviceDTOs = new List<ServiceV1DTO>();
			foreach (var service in services)
			{
				serviceDTOs.Add(await populateService(service));
			}

			return Ok(serviceDTOs);
		}

		//GET: api/Services/DepartmentAndOrDivisionId?="deptId"?="divId"
		[HttpGet]
		[Route("[action]/{depId},{divId}")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> DepartmentAndOrDivisionId(int? deptId = null, int? divId = null)
		{
			if (deptId == null && divId == null)
			{
				return BadRequest("No input given.");
			}
			var services = new List<Service>();

			if (deptId == null && divId != null)
			{
				services = await _serviceContextManager.GetServicesFromDivisionId(divId);
			}

			else if (deptId != null && divId == null)
			{
				services = await _serviceContextManager.GetServicesFromDepartmentId(deptId);
			}

			else if (deptId != null && divId != null)
			{
				services = await _serviceContextManager.GetServicesFromDivisionAndDepartmentId(divId, deptId);
			}

			if (services.Count() == 0)
			{
				return NotFound("No services found with valid arguments given.");
			}

			var serviceDTOs = new List<ServiceV1DTO>();
			foreach (var service in services)
			{
				serviceDTOs.Add(await populateService(service));
			}

			return Ok(serviceDTOs);
		}

        //GET: api/Services/Name
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Name([FromQuery] string name, int pageSize = 20, int pageNum = 0)
        {
            var services = await _serviceContextManager.GetServicesByName(name, pageSize, pageNum);

            if (services.Count == 0)
            {
                return NotFound("No services found with search query.");
            }

            var serviceDTOs = new List<ServiceV1DTO>();
            foreach (var service in services)
            {
                serviceDTOs.Add(await populateService(service));
            }

            return Ok(serviceDTOs);
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutService(int id, [FromBody] ServiceV1DTO serviceDTO)
		{
			try
			{
				if (id != serviceDTO.ServiceId)
				{
					return BadRequest();
				}

				// Check to ensure service exists before calling contextmanager method.
				var service = await _serviceContextManager.GetServiceByIdAsync(serviceDTO.ServiceId);
				if (service == null)
				{
					return NotFound();
				}

				await _serviceContextManager.PutAsync(serviceDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
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
			//Check to ensure service does not exist in database before calling contextmanager method.
			try
			{
				var service = await _serviceContextManager.GetServiceByIdAsync(serviceV1DTO.ServiceId);
				if (service != null)
				{
					return Conflict();
				}

				await _serviceContextManager.PostAsync(serviceV1DTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}


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

			foreach (var sca in service.ServiceCommunityAssociation)
			{
				int id;

				id = (int)sca.CommunityId;

				var comm = await _serviceContextManager.GetCommunityByIdAsync(id);

				if (comm == null)
				{
					continue;
				}

				serviceDTO.CommunityDTOs.Add(comm.ToCommunityV1DTO());
			}

			foreach (var sla in service.ServiceLanguageAssociation)
			{
				int id;

				id = (int)sla.LanguageId;
				var lang = await _serviceContextManager.GetLanguageByIdAsync(id);
				if (lang == null)
				{
					continue;
				}
				serviceDTO.LanguageDTOs.Add(lang.ToLanguageV1DTO());

			}

			foreach (var sla in service.ServiceLocationAssociation)
			{
				int id;
				id = (int)sla.LocationId;
				var loc = await _serviceContextManager.GetLocationByIdAsync(id);
				if (loc == null)
				{
					continue;
				}
				serviceDTO.LocationDTOs.Add(loc.ToLocationV1DTO());
			}
			return serviceDTO;
		}
	}
}
