using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    [BindProperties]
    public partial class ServiceLocationAssociation
    {
        public int ServiceLocationAssociation1 { get; set; }
        public int ServiceId { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Service Service { get; set; }
    }
}
