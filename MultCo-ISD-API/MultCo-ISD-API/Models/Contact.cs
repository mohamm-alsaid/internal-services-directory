using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Contact
    {
        public Contact()
        {
            Service = new HashSet<Service>();
        }

        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Service> Service { get; set; }
    }
}
