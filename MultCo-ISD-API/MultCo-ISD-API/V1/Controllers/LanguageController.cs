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

namespace MultCo_ISD_API.V1.Controllers
{
	[Route("services/api/v1/[controller]")]
	public class LanguageController : Controller
    {


		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IServiceContextManager _serviceContextManager;


		public LanguageController(InternalServicesDirectoryV1Context context)
		{
			_serviceContextManager = new ServiceContextManager(context);
		}

		/// <summary>
		/// Get language by id 
		/// </summary>
		/// <remarks>
		/// Returns language with a matching language id
		/// </remarks>
		/// <returns></returns>
		//[SwaggerOperation(Tags = new[] { "Reader" ,"Language"})]
		//GET: api/Services/lang?=languageId
		[HttpGet]
		[Route("[action]")]
		[ProducesResponseType(typeof(LanguageV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Language([FromQuery][Required] int languageId)
		{
			var languageDTO = await _serviceContextManager.GetLanguageByIdAsync(languageId);

			if (languageDTO == null)
			{
				return NotFound("No languages from given id found.");
			}

			return Ok(languageDTO);
		}

		[HttpPut("{languageId}")]
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
	}
}