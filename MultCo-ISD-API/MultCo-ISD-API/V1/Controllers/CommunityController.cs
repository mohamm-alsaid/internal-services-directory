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
	public class CommunityController : Controller
	{

		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IContextManager _contextManager;

		public CommunityController(InternalServicesDirectoryV1Context context)
		{
			_contextManager = new ContextManager(context);
		}


		/// <summary>
		/// Get community by id 
		/// </summary>
		/// <remarks>
		/// Returns community with a matching language id
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(CommunityV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Get(int id)
		{
			var communityDTO = await _contextManager.GetCommunityByIdAsync(id);

			if (communityDTO == null)
			{
				return NotFound("No community from given id found.");
			}

			return Ok(communityDTO.ToCommunityV1DTO());
		}


		[HttpPut("{communityId}")]
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
				var community = await _contextManager.GetCommunityByIdAsync(communityId);
				if (community == null)
				{
					return NotFound();
				}
				communityDTO.CommunityId = communityId;

				await _contextManager.PutCommunityAsync(communityDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

	}
}