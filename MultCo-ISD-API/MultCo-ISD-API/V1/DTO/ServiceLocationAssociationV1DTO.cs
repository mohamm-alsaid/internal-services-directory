using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ServiceLocationAssociationV1DTO
    {
        public int ServiceLocationAssociation { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> LocationId { get; set; }
    }
}
