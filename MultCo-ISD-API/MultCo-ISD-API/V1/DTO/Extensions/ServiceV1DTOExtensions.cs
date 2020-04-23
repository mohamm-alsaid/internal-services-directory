using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
    public static class ServiceV1DTOExtensions
    {
        public static ServiceV1DTO ToServiceV1DTO(this Service item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            ServiceV1DTO serviceV1DTO = new ServiceV1DTO();
            serviceV1DTO.CopyFromService(item);
            return serviceV1DTO;
        }

        public static Service ToService(this ServiceV1DTO item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Service service = new Service();
            service.CopyFromServiceV1DTO(item);
            return service;
        }

        public static void CopyFromServiceV1DTO(this Service to,  ServiceV1DTO from)
        {
            to.ServiceId = from.ServiceId;
            to.ProgramId = from.ProgramId;
            to.DepartmentId = from.DepartmentId;
            to.DivisionId = from.DivisionId;
            to.ServiceName = from.ServiceName;
            to.ServiceDescription = from.ServiceDescription;
            to.ExecutiveSummary = from.ExecutiveSummary;
            to.ServiceArea = from.ServiceArea;
            to.ContactId = from.ContactId;
            to.EmployeeConnectMethod = from.EmployeeConnectMethod;
            to.CustomerConnectMethod = from.CustomerConnectMethod;
            to.ExpirationDate = from.ExpirationDate;
        }

        public static void CopyFromService(this ServiceV1DTO to,  Service from)
        {
            to.ServiceId = from.ServiceId;
            to.ProgramId = from.ProgramId;
            to.DepartmentId = from.DepartmentId;
            to.DivisionId = from.DivisionId;
            to.ServiceName = from.ServiceName;
            to.ServiceDescription = from.ServiceDescription;
            to.ExecutiveSummary = from.ExecutiveSummary;
            to.ServiceArea = from.ServiceArea;
            to.ContactId = from.ContactId;
            to.EmployeeConnectMethod = from.EmployeeConnectMethod;
            to.CustomerConnectMethod = from.CustomerConnectMethod;
            to.ExpirationDate = from.ExpirationDate;

            to.ContactDTO = from.Contact?.ToContactV1DTO();
            to.DepartmentDTO = from.Department?.ToDepartmentV1DTO();
            to.DivisionDTO = from.Division?.ToDivisionV1DTO();
            to.ProgramDTO = from.Program?.ToProgramV1DTO();

        }
    }
}
