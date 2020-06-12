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
    public class DivisionController : Controller
    {

		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IContextManager _contextManager;


		public DivisionController(InternalServicesDirectoryV1Context context)
		{
			_contextManager = new ContextManager(context);
		}

		/// <summary>
		/// Get division by id 
		/// </summary>
		/// <remarks>
		/// Returns division with a matching division id
		/// </remarks>
		/// <returns></returns>
		//[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[ProducesResponseType(typeof(DivisionV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Get(int id)
		{
			var divisiontDTO = await _contextManager.GetDivisionByIdAsync(id);

			if (divisiontDTO == null)
			{
				return NotFound("No division from given id found.");
			}

			return Ok(divisiontDTO.ToDivisionV1DTO());
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
				var division = await _contextManager.GetDivisionByIdAsync(divisionId);
				if (division == null)
				{
					return NotFound();
				}
				divisionDTO.DivisionId = divisionId;

				await _contextManager.PutDivisionAsync(divisionDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}