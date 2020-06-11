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
	public class LocationTypeController : Controller
	{
		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IServiceContextManager _serviceContextManager;

		public LocationTypeController(InternalServicesDirectoryV1Context context)
		{
			_serviceContextManager = new ServiceContextManager(context);
		}


		/// <summary>
		/// Get locationType by id 
		/// </summary>
		/// <remarks>
		/// Returns locationType with a matching locationType id
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(LocationTypeV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> LocationType([FromQuery][Required] int locationTypetId)
		{
			var locationTypeDTO = await _serviceContextManager.GetLocationTypeByIdAsync(locationTypetId);

			if (locationTypeDTO == null)
			{
				return NotFound("No locationType from given id found.");
			}

			return Ok(locationTypeDTO);
		}



		[HttpPut("{locationTypeId}")]
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
	}
}