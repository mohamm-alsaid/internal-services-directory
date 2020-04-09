using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISD.V1.DTO
{
    public class ServiceLocationAssociationV1DTO
    {
        public int ServiceLocationAssociation1 { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> LocationID { get; set; }
    }
}