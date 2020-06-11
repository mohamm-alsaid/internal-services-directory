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
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
﻿
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
        /// <summary>
        /// Get all active services
        /// </summary>
        /// <remarks>
        /// Get services with pagination
        /// </remarks>
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
        /// <summary>
        /// Get a particular service by service ID.
        /// </summary>
        /// <remarks>
        /// Returns the service with matching id, get all information associated with it
        /// </remarks>
        [SwaggerOperation(Tags = new[] { "Reader" })]

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
    
        /// <summary>
        /// Get service by language 
        /// </summary>
        /// <remarks>
        /// Returns all services with a matching language 
        /// </remarks>
        /// <param name="lang"></param>
        /// <returns></returns>
        [SwaggerOperation(Tags = new[] { "Reader" })]
		//GET: api/Services/lang?="language"
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Language([FromQuery][Required] string lang, int pageSize = 20, int pageNum = 0)
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

			var services = await _serviceContextManager.GetServicesFromIdListPaginated(serviceIds, pageSize, pageNum);
			var serviceDTOs = new List<ServiceV1DTO>();
			foreach (var service in services)
			{
				serviceDTOs.Add(await populateService(service));
			}

			return Ok(serviceDTOs);
		}

		/// <summary>
		/// Get service by community name
		/// </summary>
		/// <remarks>
		/// get service with a matching community name
		/// </remarks>
		/// <param name="community"></param>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		//GET: api/Services/Community?="community"
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Community([FromQuery][Required] string community)
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

				var ids = new List<int>(); //nullable for now, schema has these ids nullable at the moment, will probably fix this in next sprint
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
		/// <summary>
		/// Get service by building Id
		/// </summary>
		/// <remarks>
		/// get service with a matching build Id
		/// </remarks>
		/// <param name="buildingId"></param>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]

		// GET: api/Service/BuildingId
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> BuildingId([FromQuery][Required] string buildingId)
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

		/// <summary>
		/// Get service by program Id
		/// </summary>
		/// <remarks>
		/// get service with a matching program Id
		/// </remarks>
		/// <param name="programId"></param>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]

		//GET: api/Services/Program?="programId"
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Program([FromQuery][Required] int programId)
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
		/// <summary>
		/// Get service by a division and or department Id
		/// </summary>
		/// <remarks>
		/// paginated
		/// </remarks>
		[SwaggerOperation(Tags = new[] { "Reader" })]

		//GET: api/Services/DepartmentAndOrDivisionId?="deptId"?="divId"
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> DepartmentAndOrDivisionId([FromQuery] int? deptId = null, int? divId = null, int pageSize = 20, int pageNum = 0)
		{
			if (deptId == null && divId == null)
			{
				return BadRequest("No input given.");
			}
			var services = new List<Service>();

			if (deptId == null && divId != null)
			{
				services = await _serviceContextManager.GetServicesFromDivisionId(divId, pageSize, pageNum);
			}

			else if (deptId != null && divId == null)
			{
				services = await _serviceContextManager.GetServicesFromDepartmentId(deptId, pageSize, pageNum);
			}

			else if (deptId != null && divId != null)
			{
				services = await _serviceContextManager.GetServicesFromDivisionAndDepartmentId(divId, deptId, pageSize, pageNum);
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

		/// <summary>
		/// Get service by name 
		/// </summary>
		/// <remarks>
		/// get service with a matching name ( even if the name is a substring of another ) <br/>
		/// matching the regex: .*a_string.* or SQL: like %a_string%
		/// </remarks>
		[SwaggerOperation(Tags = new[] { "Reader" })]

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
		/// <summary>
		/// Update a service with a certain ID
		/// </summary>
		/// <remarks>
		/// Add a service if it doesn't already exist and update a service if it exist
		/// </remarks>
		[SwaggerOperation(Tags = new[] { "Writer" })]

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
				// Check to ensure service exists before calling contextmanager method.
				var service = await _serviceContextManager.GetServiceByIdAsync(id);
				if (service == null)
				{
					return NotFound();
				}
				serviceDTO.ServiceId = id;

				await _serviceContextManager.PutAsync(serviceDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		/// <summary>
		/// Post a new service
		/// </summary>
		/// <remarks>
		/// Adds a service
		/// </remarks>
		[SwaggerOperation(Tags = new[] { "Writer" })]

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
		// ----------------- PUT lang ------------
		[HttpPut("Language/{languageId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutLanguage(int languageId, [FromBody] LanguageV1DTO languageDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var language = await _serviceContextManager.GetLanguageByIdAsync(languageId);
				if (language == null)
				{
					return NotFound();
				}
				languageDTO.LanguageId = languageId;

				await _serviceContextManager.PutLanguageAsync(languageDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("Community/{communityId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutCommunity(int communityId, [FromBody] CommunityV1DTO communityDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var community = await _serviceContextManager.GetCommunityByIdAsync(communityId);
				if (community == null)
				{
					return NotFound();
				}
				communityDTO.CommunityId = communityId;

				await _serviceContextManager.PutCommunityAsync(communityDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("Division/{divisionId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutDivision(int divisionId, [FromBody] DivisionV1DTO divisionDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var division = await _serviceContextManager.GetDivisionByIdAsync(divisionId);
				if (division == null)
				{
					return NotFound();
				}
				divisionDTO.DivisionId = divisionId;

				await _serviceContextManager.PutDivisionAsync(divisionDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("Department/{departmentId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutDepartment(int departmentId, [FromBody] DepartmentV1DTO departmentDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var department = await _serviceContextManager.GetDepartmentByIdAsync(departmentId);
				if (department == null)
				{
					return NotFound();
				}
				departmentDTO.DepartmentId = departmentId;

				await _serviceContextManager.PutDepartmentAsync(departmentDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("Program/{programId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutProgram(int programId, [FromBody] ProgramV1DTO programDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var program = await _serviceContextManager.GetProgramByIdAsync(programId);
				if (program == null)
				{
					return NotFound();
				}
				programDTO.ProgramId = programId;

				await _serviceContextManager.PutProgramAsync(programDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		[HttpPut("Location/{locationId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutLocation(int locationId, [FromBody] LocationV1DTO locationDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var location = await _serviceContextManager.GetLocationByIdAsync(locationId);
				if (location == null)
				{
					return NotFound();
				}
				locationDTO.LocationId = locationId;

				await _serviceContextManager.PutLocationAsync(locationDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("LocationType/{locationTypeId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutLocationType(int locationTypeId, [FromBody] LocationTypeV1DTO locationTypeDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var locationType = await _serviceContextManager.GetLocationTypeByIdAsync(locationTypeId);
				if (locationType == null)
				{
					return NotFound();
				}
				locationTypeDTO.LocationTypeId = locationTypeId;

				await _serviceContextManager.PutLocationTypeAsync(locationTypeDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		[HttpPut("Contact/{contactId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Writer")]
#endif
		public async Task<IActionResult> PutContact(int contactId, [FromBody] ContactV1DTO contactDTO)
		{
			try
			{
				// Check to ensure service exists before calling contextmanager method.
				var contact = await _serviceContextManager.GetContactByIdAsync(contactId);
				if (contact == null)
				{
					return NotFound();
				}
				contactDTO.ContactId = contactId;

				await _serviceContextManager.PutContactAsync(contactDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		// ----------------- end put -------------------
		// ----------------- start get -------------------
		/// <summary>
		/// Get language by id 
		/// </summary>
		/// <remarks>
		/// Returns language with a matching language id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		//GET: api/Services/lang?=languageId
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetLanguage([FromQuery][Required] int languageId)
		{
			var languageDTO = await _serviceContextManager.GetLanguageByIdAsync(languageId);

			if (languageDTO == null)
			{
				return NotFound("No languages from given id found.");
			}

			return Ok(languageDTO);
		}
		/// <summary>
		/// Get community by id 
		/// </summary>
		/// <remarks>
		/// Returns community with a matching language id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetCommunity([FromQuery][Required] int communityId)
		{
			var communityDTO = await _serviceContextManager.GetCommunityByIdAsync(communityId);

			if (communityDTO == null)
			{
				return NotFound("No community from given id found.");
			}

			return Ok(communityDTO);
		}

		/// <summary>
		/// Get department by id 
		/// </summary>
		/// <remarks>
		/// Returns department with a matching department id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetDepartment([FromQuery][Required] int departmentId)
		{
			var departmentDTO = await _serviceContextManager.GetDepartmentByIdAsync(departmentId);

			if (departmentDTO == null)
			{
				return NotFound("No department from given id found.");
			}

			return Ok(departmentDTO);
		}

		/// <summary>
		/// Get division by id 
		/// </summary>
		/// <remarks>
		/// Returns division with a matching division id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetDivision([FromQuery][Required] int divisiontId)
		{
			var divisiontDTO = await _serviceContextManager.GetDivisionByIdAsync(divisiontId);

			if (divisiontDTO == null)
			{
				return NotFound("No division from given id found.");
			}

			return Ok(divisiontDTO);
		}

		/// <summary>
		/// Get program by id 
		/// </summary>
		/// <remarks>
		/// Returns program with a matching program id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetProgram([FromQuery][Required] int programtId)
		{
			var programDTO = await _serviceContextManager.GetProgramByIdAsync(programtId);

			if (programDTO == null)
			{
				return NotFound("No program from given id found.");
			}

			return Ok(programDTO);
		}

		/// <summary>
		/// Get location by id 
		/// </summary>
		/// <remarks>
		/// Returns location with a matching location id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetLocation([FromQuery][Required] int locationtId)
		{
			var locationDTO = await _serviceContextManager.GetLocationByIdAsync(locationtId);

			if (locationDTO == null)
			{
				return NotFound("No location from given id found.");
			}

			return Ok(locationDTO);
		}

		/// <summary>
		/// Get locationType by id 
		/// </summary>
		/// <remarks>
		/// Returns locationType with a matching locationType id
		/// </remarks>
		/// <returns></returns>
		[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(ServiceV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> GetLocationType([FromQuery][Required] int locationTypetId)
		{
			var locationTypeDTO = await _serviceContextManager.GetLocationTypeByIdAsync(locationTypetId);

			if (locationTypeDTO == null)
			{
				return NotFound("No locationType from given id found.");
			}

			return Ok(locationTypeDTO);
		}
		// ----------------- end get -------------------
	}
}
