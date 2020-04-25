using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Program
    {
        public Program()
        {
            Service = new HashSet<Service>();
        }

        public int ProgramId { get; set; }
        public string SponsorName { get; set; }
        public string OfferType { get; set; }
        public string ProgramName { get; set; }

        public string ProgramOfferNumber { get; set; }

        public virtual ICollection<Service> Service { get; set; }
    }
}
