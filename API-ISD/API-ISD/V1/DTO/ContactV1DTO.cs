using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISD.V1.DTO
{
    public class ContactV1DTO
    {
        public int ContactID { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}