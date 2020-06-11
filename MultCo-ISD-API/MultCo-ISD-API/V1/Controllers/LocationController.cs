using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.DTO;
using MultCo_ISD_API.V1.ControllerContexts;
using System.ComponentModel.DataAnnotations;

namespace MultCo_ISD_API.V1.Controllers
{
	[Route("services/api/v1/[controller]")]
	public class LocationController : Controller
    {
		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IServiceContextManager _serviceContextManager;

		public LocationController(InternalServicesDirectoryV1Context context)
		{
			_serviceContextManager = new ServiceContextManager(context);
		}


		/// <summary>
		/// Get location by id 
		/// </summary>
		/// <remarks>
		/// Returns location with a matching location id
		/// </remarks>
		/// <returns></returns>
		//[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(LocationV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Location([FromQuery][Required] int locationtId)
		{
			var locationDTO = await _serviceContextManager.GetLocationByIdAsync(locationtId);

			if (locationDTO == null)
			{
				return NotFound("No location from given id found.");
			}

			return Ok(locationDTO);
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

	
	}
}