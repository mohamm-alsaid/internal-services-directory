using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Language
    {
        public Language()
        {
            ServiceLanguageAssociation = new HashSet<ServiceLanguageAssociation>();
        }

        public int LanguageId { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<ServiceLanguageAssociation> ServiceLanguageAssociation { get; set; }
    }
}
