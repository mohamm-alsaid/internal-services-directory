using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Service
    {
        public Service()
        {
            ServiceCommunityAssociation = new HashSet<ServiceCommunityAssociation>();
            ServiceLanguageAssociation = new HashSet<ServiceLanguageAssociation>();
            ServiceLocationAssociation = new HashSet<ServiceLocationAssociation>();
        }

        public int ServiceId { get; set; }
        public int? ProgramId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ExecutiveSummary { get; set; }
        public string ServiceArea { get; set; }
        public int? ContactId { get; set; }
        public string EmployeeConnectMethod { get; set; }
        public string CustomerConnectMethod { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public Boolean Active { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Department Department { get; set; }
        public virtual Division Division { get; set; }
        public virtual Program Program { get; set; }
        public virtual ICollection<ServiceCommunityAssociation> ServiceCommunityAssociation { get; set; }
        public virtual ICollection<ServiceLanguageAssociation> ServiceLanguageAssociation { get; set; }
        public virtual ICollection<ServiceLocationAssociation> ServiceLocationAssociation { get; set; }
    }
}
