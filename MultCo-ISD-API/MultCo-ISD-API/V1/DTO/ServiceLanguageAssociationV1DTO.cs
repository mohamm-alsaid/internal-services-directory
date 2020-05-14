using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ServiceLanguageAssociationV1DTO
    {
        public int ServiceLanguageAssociation1 { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> LanguageId { get; set; }
    }
}
