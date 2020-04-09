using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISD.V1.DTO
{
    public class ServiceV1DTO
    {
        public int ServiceID { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public Nullable<int> DivisionID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ExecutiveSummary { get; set; }
        public string ServiceArea { get; set; }
        public Nullable<int> ContactID { get; set; }
        public string EmployeeConnectMethod { get; set; }
        public string CustomerConnectMethod { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }
}