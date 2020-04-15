using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Community
    {
        public Community()
        {
            ProgramCommunityAssociation = new HashSet<ProgramCommunityAssociation>();
        }

        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityDescription { get; set; }

        public virtual ICollection<ProgramCommunityAssociation> ProgramCommunityAssociation { get; set; }
    }
}
