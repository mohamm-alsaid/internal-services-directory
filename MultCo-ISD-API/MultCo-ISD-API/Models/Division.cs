using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Division
    {
        public Division()
        {
            Service = new HashSet<Service>();
        }

        public int DivisionId { get; set; }
        public int DivisionCode { get; set; }
        public string DivisionName { get; set; }

        public virtual ICollection<Service> Service { get; set; }
    }
}
