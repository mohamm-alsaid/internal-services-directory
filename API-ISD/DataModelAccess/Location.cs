//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModelAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            this.ServiceLocationAssociations = new HashSet<ServiceLocationAssociation>();
        }
    
        public int locationID { get; set; }
        public int locationTypeID { get; set; }
        public string locationName { get; set; }
        public string buildingID { get; set; }
        public string locationAddress { get; set; }
        public string roomNumber { get; set; }
        public string floorNumber { get; set; }
    
        public virtual LocationType LocationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceLocationAssociation> ServiceLocationAssociations { get; set; }
    }
}
