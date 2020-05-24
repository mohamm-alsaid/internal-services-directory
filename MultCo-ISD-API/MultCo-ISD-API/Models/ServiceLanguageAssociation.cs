using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class ServiceLanguageAssociation
    {
        public int ServiceLanguageAssociation1 { get; set; }
        public int ServiceId { get; set; }
        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }
        public virtual Service Service { get; set; }
    }
}
