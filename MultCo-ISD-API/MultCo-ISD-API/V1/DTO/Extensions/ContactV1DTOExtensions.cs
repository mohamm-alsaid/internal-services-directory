using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ContactV1DTOExtensions
	{
		public static ContactV1DTO ToContactV1DTO(this Contact item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ContactV1DTO contactV1DTO = new ContactV1DTO();
			contactV1DTO.CopyFromContact(item);
			return contactV1DTO;
		}

		public static Contact ToContact(this ContactV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Contact contact = new Contact();
			contact.CopyFromContactV1DTO(item);
			return contact;
		}

		public static void CopyFromContactV1DTO(this Contact to, ContactV1DTO from)
		{
			to.ContactId = from.ContactId;
			to.ContactName = from.ContactName;
			to.PhoneNumber = from.PhoneNumber;
			to.EmailAddress = from.EmailAddress;
		}

		public static void CopyFromContact(this ContactV1DTO to, Contact from)
		{
			to.ContactId = from.ContactId;
			to.ContactName = from.ContactName;
			to.PhoneNumber = from.PhoneNumber;
			to.EmailAddress = from.EmailAddress;
		}
	}
}
