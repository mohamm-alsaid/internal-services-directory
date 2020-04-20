using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ServiceLocationAssociationV1DTOExtensions
	{
        public static ServiceLocationAssociationV1DTO ToServiceLocationAssociationV1DTO(this ServiceLocationAssociation item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ServiceLocationAssociationV1DTO serviceLocationAssociationV1DTO = new ServiceLocationAssociationV1DTO();
            serviceLocationAssociationV1DTO.CopyFromServiceLocationAssociation(item);
            return serviceLocationAssociationV1DTO;
        }

        public static ServiceLocationAssociation ToServiceLocationAssociation(this ServiceLocationAssociationV1DTO item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ServiceLocationAssociation serviceLocationAssociation = new ServiceLocationAssociation();
            serviceLocationAssociation.CopyFromServiceLocationAssociationV1DTO(item);
            return serviceLocationAssociation;
        }

        public static void CopyFromServiceLocationAssociationV1DTO(this ServiceLocationAssociation to, ServiceLocationAssociationV1DTO from)
        {
            to.ServiceLocationAssociation1 = from.ServiceLocationAssociation;
            to.ServiceId = from.ServiceID;
            to.LocationId = from.LocationID;
        }

        public static void CopyFromServiceLocationAssociation(this ServiceLocationAssociationV1DTO to, ServiceLocationAssociation from)
        {
            to.ServiceLocationAssociation = from.ServiceLocationAssociation1;
            to.ServiceID = from.ServiceId;
            to.LocationID = from.LocationId;
        }
    }
}
