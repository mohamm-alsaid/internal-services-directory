using MultCo_ISD_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ServiceV1DTO
    {
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

        //public virtual ICollection<ProgramCommunityAssociationV1DTO> ProgramCommunityAssociationDTO { get; set; }
        //public virtual ICollection<ServiceLanguageAssociationV1DTO> ServiceLanguageAssociationDTO { get; set; }
        //public virtual ICollection<ServiceLocationAssociationV1DTO> ServiceLocationAssociationDTO { get; set; }
    }

}
