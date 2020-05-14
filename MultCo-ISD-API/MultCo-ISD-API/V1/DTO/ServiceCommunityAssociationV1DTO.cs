using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ServiceCommunityAssociationV1DTO
    {
        public int ServiceCommunityAssociationId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> CommunityId { get; set; }
    }
}
