//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorldFestSolution.WebAPI.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ParticipantInvite
    {
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public int ParticipantId { get; set; }
        public int FestivalId { get; set; }
        public bool IsAccepted { get; set; }
    
        public virtual Festival Festival { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
