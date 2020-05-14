using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class ServiceLanguageAssociationV1DTOExtensions
	{
        public static ServiceLanguageAssociationV1DTO ToServiceLanguageAssociationV1DTO(this ServiceLanguageAssociation item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ServiceLanguageAssociationV1DTO serviceLanguageAssociationV1DTO = new ServiceLanguageAssociationV1DTO();
            serviceLanguageAssociationV1DTO.CopyFromServiceLanguageAssociation(item);
            return serviceLanguageAssociationV1DTO;
        }

        public static ServiceLanguageAssociation ToServiceLanguageAssociation(this ServiceLanguageAssociationV1DTO item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ServiceLanguageAssociation serviceLanguageAssociation = new ServiceLanguageAssociation();
            serviceLanguageAssociation.CopyFromServiceLanguageAssociationV1DTO(item);
            return serviceLanguageAssociation;
        }

        public static void CopyFromServiceLanguageAssociationV1DTO(this ServiceLanguageAssociation to, ServiceLanguageAssociationV1DTO from)
        {
            to.ServiceLanguageAssociation1 = from.ServiceLanguageAssociation1;
            to.ServiceId = from.ServiceId;
            to.LanguageId = from.LanguageId;
        }

        public static void CopyFromServiceLanguageAssociation(this ServiceLanguageAssociationV1DTO to, ServiceLanguageAssociation from)
        {
            to.ServiceLanguageAssociation1 = from.ServiceLanguageAssociation1;
            to.ServiceId = from.ServiceId;
            to.LanguageId = from.LanguageId;
        }
    }
}
