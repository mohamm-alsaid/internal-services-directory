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
	public class DepartmentController : Controller
	{

		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IServiceContextManager _serviceContextManager;

		public DepartmentController(InternalServicesDirectoryV1Context context)
		{
			_serviceContextManager = new ServiceContextManager(context);
		}


		/// <summary>
		/// Get department by id 
		/// </summary>
		/// <remarks>
		/// Returns department with a matching department id
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(DepartmentV1DTO), (int)HttpStatusCode.OK)]
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



		[HttpPut("{departmentId}")]
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
	}
}