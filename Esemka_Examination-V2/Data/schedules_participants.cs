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
    
    public partial class schedules_participants
    {
        public int id { get; set; }
        public int schedule_id { get; set; }
        public int participant_id { get; set; }
        public System.DateTime created_at { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
    
        public virtual schedule schedule { get; set; }
        public virtual user user { get; set; }
    }
}
