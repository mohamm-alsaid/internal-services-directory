using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISD.V1.DTO
{
    public class ServiceLanguageAssociationV1DTO
    {
        public int ServiceLanguageAssociation1 { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> LanguageID { get; set; }
    }
}