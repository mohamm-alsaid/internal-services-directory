using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    [BindProperties]
    public partial class Program
    {
        public Program()
        {
            Service = new HashSet<Service>();
        }

        public int ProgramId { get; set; }
        public string SponsorName { get; set; }
        public string OfferType { get; set; }

        public virtual ICollection<Service> Service { get; set; }
    }
}
