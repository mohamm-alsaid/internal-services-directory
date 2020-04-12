using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class ProgramCommunityAssociation
    {
        public int ProgramCommunityAssociationId { get; set; }
        public int? ServiceId { get; set; }
        public int? CommunityId { get; set; }

        public virtual Community Community { get; set; }
        public virtual Service Service { get; set; }
    }
}
