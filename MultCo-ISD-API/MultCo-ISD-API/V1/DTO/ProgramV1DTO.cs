using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultCo_ISD_API.V1.DTO
{
    public class ProgramV1DTO
    {
        public int ProgramID { get; set; }
        public string SponsorName { get; set; }
        public string OfferType { get; set; }
        public string ProgramName { get; set; }

        public string ProgramOfferNumber { get; set; }
    }
}
