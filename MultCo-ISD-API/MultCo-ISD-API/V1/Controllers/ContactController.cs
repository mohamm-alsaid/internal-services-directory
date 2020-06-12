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
	public class ContactController : Controller
	{
		private readonly InternalServicesDirectoryV1Context _context;
		private readonly IContextManager _contextManager;

		public ContactController(InternalServicesDirectoryV1Context context)
		{
			_contextManager = new ContextManager(context);
		}



		/// <summary>
		/// Get contact by id 
		/// </summary>
		/// <remarks>
		/// Returns contact with a matching contact id
		/// </remarks>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(ContactV1DTO), (int)HttpStatusCode.OK)]
		[ProducesResponseType(404)]
#if AUTH
        [Authorize(Policy = "Reader")]
#endif
		public async Task<IActionResult> Get( int id)
		{
			var contactDTO = await _contextManager.GetContactByIdAsync(id);

			if (contactDTO == null)
			{
				return NotFound("No contact from given id found.");
			}

			return Ok(contactDTO.ToContactV1DTO());
		}

		[HttpPut("{contactId}")]
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
				var contact = await _contextManager.GetContactByIdAsync(contactId);
				if (contact == null)
				{
					return NotFound();
				}
				contactDTO.ContactId = contactId;

				await _contextManager.PutContactAsync(contactDTO);
				return NoContent();
			}
			catch (Exception e)
			{
				throw e;
			}
		}



	}
}