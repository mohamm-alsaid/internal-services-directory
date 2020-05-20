using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    [BindProperties]
    public partial class ServiceCommunityAssociation
    {
        public int ServiceCommunityAssociationId { get; set; }
        public int ServiceId { get; set; }
        public int CommunityId { get; set; }

        public virtual Community Community { get; set; }
        public virtual Service Service { get; set; }
    }
}
