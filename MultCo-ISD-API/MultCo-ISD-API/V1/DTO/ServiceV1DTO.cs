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

        public Boolean Active { get; set; }
    }
}
