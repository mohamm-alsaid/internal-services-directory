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
    public class ProgramController : Controller
    {

        private readonly InternalServicesDirectoryV1Context _context;
        private readonly IContextManager _contextManager;

		public ProgramController(InternalServicesDirectoryV1Context context)
		{
			_contextManager = new ContextManager(context);
		}



		/// <summary>
		/// Get program by id 
		/// </summary>
		/// <remarks>
		/// Returns program with a matching program id
		/// </remarks>
		/// <returns></returns>
		//[SwaggerOperation(Tags = new[] { "Reader" })]
		[HttpGet]
		[ProducesResponseType(typeof(ProgramV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Get(int id)
		{
			var programDTO = await _contextManager.GetProgramByIdAsync(id);

			if (programDTO == null)
			{
				return NotFound("No program from given id found.");
			}

			return Ok(programDTO.ToProgramV1DTO());
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
				var program = await _contextManager.GetProgramByIdAsync(programId);
				if (program == null)
				{
					return NotFound();
				}
				programDTO.ProgramId = programId;

				await _contextManager.PutProgramAsync(programDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}


	}
}