using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ProgramCommunityAssociationV1DTOExtensions
	{
		public static ProgramCommunityAssociationV1DTO ToProgramCommunityAssociationV1DTO(this ProgramCommunityAssociation item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ProgramCommunityAssociationV1DTO programCommunityAssociationV1DTO = new ProgramCommunityAssociationV1DTO();
			programCommunityAssociationV1DTO.CopyFromProgramCommunityAssociation(item);
			return programCommunityAssociationV1DTO;
		}

		public static ProgramCommunityAssociation toProgramCommunityAssociation(this ProgramCommunityAssociationV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ProgramCommunityAssociation programCommunityAssociation = new ProgramCommunityAssociation();
			programCommunityAssociation.CopyFromProgramCommunityAssociationV1DTO(item);
			return programCommunityAssociation;
		}

		public static void CopyFromProgramCommunityAssociationV1DTO(this ProgramCommunityAssociation to, ProgramCommunityAssociationV1DTO from)
		{
			to.ServiceId = from.ServiceID;
			to.CommunityId = from.CommunityID;
		}

		public static void CopyFromProgramCommunityAssociation(this ProgramCommunityAssociationV1DTO to, ProgramCommunityAssociation from)
		{
			to.ServiceID = from.ServiceId;
			to.CommunityID = from.CommunityId;
		}
	}
}
