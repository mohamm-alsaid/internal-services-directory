using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ProgramV1DTOExtensions
	{
		public static ProgramV1DTO ToProgramV1DTO(this Program item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ProgramV1DTO programV1DTO = new ProgramV1DTO();
			programV1DTO.CopyFromProgram(item);
			return programV1DTO;
		}

		public static Program toProgram(this ProgramV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Program program = new Program();
			program.CopyFromProgramV1DTO(item);
			return program;
		}

		public static void CopyFromProgramV1DTO(this Program to, ProgramV1DTO from)
		{
			to.ProgramId = from.ProgramID;
			to.SponsorName = from.SponsorName;
			to.OfferType = from.OfferType;
			to.ProgramName = from.ProgramName;
			to.ProgramOfferNumber = from.ProgramOfferNumber;
		}

		public static void CopyFromProgram(this ProgramV1DTO to, Program from)
		{
			to.ProgramID = from.ProgramId;
			to.SponsorName = from.SponsorName;
			to.OfferType = from.OfferType;
			to.ProgramName = from.ProgramName;
			to.ProgramOfferNumber = from.ProgramOfferNumber;
		}
	}
}
