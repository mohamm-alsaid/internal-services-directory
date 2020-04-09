using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISD.V1.DTO
{
    public class LocationV1DTO
    {
        public int LocationID { get; set; }
        public int LocationTypeID { get; set; }
        public string LocationName { get; set; }
        public string BuildingID { get; set; }
        public string LocationAddress { get; set; }
        public string RoomNumber { get; set; }
        public string FloorNumber { get; set; }
    }
}