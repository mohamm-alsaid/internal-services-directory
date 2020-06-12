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
		private readonly IContextManager _contextManager;

		public DepartmentController(InternalServicesDirectoryV1Context context)
		{
			_contextManager = new ContextManager(context);
		}


		/// <summary>
		/// Get department by id 
		/// </summary>
		/// <remarks>
		/// Returns department with a matching department id
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(DepartmentV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Get(int id)
		{
			var departmentDTO = await _contextManager.GetDepartmentByIdAsync(id);

			if (departmentDTO == null)
			{
				return NotFound("No department from given id found.");
			}

			return Ok(departmentDTO.ToDepartmentV1DTO());
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
				var department = await _contextManager.GetDepartmentByIdAsync(departmentId);
				if (department == null)
				{
					return NotFound();
				}
				departmentDTO.DepartmentId = departmentId;

				await _contextManager.PutDepartmentAsync(departmentDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}