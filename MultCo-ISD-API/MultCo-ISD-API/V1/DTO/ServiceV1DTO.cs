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

            //Initialize new collection
            CommunityDTOs = new List<CommunityV1DTO>();
            LanguageDTOs = new List<LanguageV1DTO>();
            LocationDTOs = new List<LocationV1DTO>();
        }
        // To hide this from swagger (ignores obsolete attributes)
        [Obsolete]
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

        //Experimental collections to return related objects
        public List<CommunityV1DTO> CommunityDTOs { get; set; }
        public List<LanguageV1DTO> LanguageDTOs { get; set; }
        public List<LocationV1DTO> LocationDTOs { get; set; }
    }
}
