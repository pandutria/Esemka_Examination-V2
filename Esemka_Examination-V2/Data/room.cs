//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Esemka_Examination_V2.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public room()
        {
            this.schedules = new HashSet<schedule>();
        }
    
        public int id { get; set; }
        public string code { get; set; }
        public int capacity { get; set; }
        public System.DateTime created_at { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public string imagePath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schedule> schedules { get; set; }
    }
}
