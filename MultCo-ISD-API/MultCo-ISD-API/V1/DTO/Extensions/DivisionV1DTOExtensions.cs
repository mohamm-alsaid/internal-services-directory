using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class DivisionV1DTOExtensions
	{
		public static DivisionV1DTO ToDivisionV1DTO(this Division item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			DivisionV1DTO divisionV1DTO = new DivisionV1DTO();
			divisionV1DTO.CopyFromDivision(item);
			return divisionV1DTO;
		}

		public static Division ToDivision(this DivisionV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Division division = new Division();
			division.CopyFromDivisionV1DTO(item);
			return division;
		}

		public static void CopyFromDivisionV1DTO(this Division to, DivisionV1DTO from)
		{
			to.DivisionId = from.DivisionId;
			to.DivisionCode = from.DivisionCode;
			to.DivisionName = from.DivisionName;
		}

		public static void CopyFromDivision(this DivisionV1DTO to, Division from)
		{
			to.DivisionId = from.DivisionId;
			to.DivisionCode = from.DivisionCode;
			to.DivisionName = from.DivisionName;
		}
	}
}
