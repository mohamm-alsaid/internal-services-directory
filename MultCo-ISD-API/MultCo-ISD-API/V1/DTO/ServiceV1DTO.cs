using MultCo_ISD_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ServiceV1DTO
    {
        public ServiceV1DTO()
        {
            ServiceCommunityAssociationDTOs = new List<ServiceCommunityAssociationV1DTO>();
            ServiceLanguageAssociationDTOs = new List<ServiceLanguageAssociationV1DTO>();
            ServiceLocationAssociationDTOs = new List<ServiceLocationAssociationV1DTO>();
        }

        public int ServiceId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ExecutiveSummary { get; set; }
        public string ServiceArea { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string EmployeeConnectMethod { get; set; }
        public string CustomerConnectMethod { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Boolean Active { get; set; }

        public ContactV1DTO ContactDTO { get; set; }
        public DepartmentV1DTO DepartmentDTO { get; set; }
        public DivisionV1DTO DivisionDTO { get; set; }
        public ProgramV1DTO ProgramDTO { get; set; }

        public List<ServiceCommunityAssociationV1DTO> ServiceCommunityAssociationDTOs { get; set; }
        public List<ServiceLanguageAssociationV1DTO> ServiceLanguageAssociationDTOs { get; set; }
        public List<ServiceLocationAssociationV1DTO> ServiceLocationAssociationDTOs { get; set; }
    }
}
