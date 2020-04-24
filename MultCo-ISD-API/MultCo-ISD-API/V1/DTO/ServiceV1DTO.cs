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
            CommunityDTOs = new HashSet<CommunityV1DTO>();
            LanguageDTOs = new HashSet<LanguageV1DTO>();
            LocationDTOs = new HashSet<LocationV1DTO>();
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

        public ContactV1DTO ContactDTO { get; set; }
        public DepartmentV1DTO DepartmentDTO { get; set; }
        public DivisionV1DTO DivisionDTO { get; set; }
        public ProgramV1DTO ProgramDTO { get; set; }

        public HashSet<CommunityV1DTO> CommunityDTOs { get; set; }
        public HashSet<LanguageV1DTO> LanguageDTOs { get; set; }
        public HashSet<LocationV1DTO> LocationDTOs { get; set; }
        public Boolean Active { get; set; }
    }

}
