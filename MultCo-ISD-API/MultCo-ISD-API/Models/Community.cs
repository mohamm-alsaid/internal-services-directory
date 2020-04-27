using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Community
    {
        public Community()
        {
            ServiceCommunityAssociation = new HashSet<ServiceCommunityAssociation>();
        }

        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityDescription { get; set; }

        public virtual ICollection<ServiceCommunityAssociation> ServiceCommunityAssociation { get; set; }
    }
}
