using System;
using System.Collections.Generic;

namespace MultCo_ISD_API.Models
{
    public partial class Location
    {
        public Location()
        {
            ServiceLocationAssociation = new HashSet<ServiceLocationAssociation>();
        }

        public int LocationId { get; set; }
        public int LocationTypeId { get; set; }
        public string LocationName { get; set; }
        public string BuildingId { get; set; }
        public string LocationAddress { get; set; }
        public string RoomNumber { get; set; }
        public string FloorNumber { get; set; }

        public virtual LocationTypeV1DTOValidator LocationType { get; set; }
        public virtual ICollection<ServiceLocationAssociation> ServiceLocationAssociation { get; set; }
    }
}
