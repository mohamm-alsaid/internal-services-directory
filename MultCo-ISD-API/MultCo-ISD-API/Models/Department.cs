using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Department
    {
        public Department()
        {
            Service = new HashSet<Service>();
        }

        public int DepartmentId { get; set; }
        public int DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<Service> Service { get; set; }
    }
}
