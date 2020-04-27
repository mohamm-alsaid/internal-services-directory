using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ServiceCommunityAssociationV1DTOExtensions
	{
		public static ServiceCommunityAssociationV1DTO ToServiceCommunityAssociationV1DTO(this ServiceCommunityAssociation item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ServiceCommunityAssociationV1DTO serviceCommunityAssociationV1DTO = new ServiceCommunityAssociationV1DTO();
			serviceCommunityAssociationV1DTO.CopyFromServiceCommunityAssociation(item);
			return serviceCommunityAssociationV1DTO;
		}

		public static ServiceCommunityAssociation toServiceCommunityAssociation(this ServiceCommunityAssociationV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			ServiceCommunityAssociation serviceCommunityAssociation = new ServiceCommunityAssociation();
			serviceCommunityAssociation.CopyFromServiceCommunityAssociationV1DTO(item);
			return serviceCommunityAssociation;
		}

		public static void CopyFromServiceCommunityAssociationV1DTO(this ServiceCommunityAssociation to, ServiceCommunityAssociationV1DTO from)
		{
			to.ServiceId = from.ServiceID;
			to.CommunityId = from.CommunityID;
		}

		public static void CopyFromServiceCommunityAssociation(this ServiceCommunityAssociationV1DTO to, ServiceCommunityAssociation from)
		{
			to.ServiceID = from.ServiceId;
			to.CommunityID = from.CommunityId;
		}
	}
}
