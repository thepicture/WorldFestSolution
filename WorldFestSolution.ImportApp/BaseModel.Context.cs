//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorldFestSolution.ImportApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WorldFestBaseEntities : DbContext
    {
        public WorldFestBaseEntities()
            : base("name=WorldFestBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Festival> Festivals { get; set; }
        public virtual DbSet<FestivalComment> FestivalComments { get; set; }
        public virtual DbSet<FestivalProgram> FestivalPrograms { get; set; }
        public virtual DbSet<FestivalRating> FestivalRatings { get; set; }
        public virtual DbSet<ParticipantInvite> ParticipantInvites { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRating> UserRatings { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
    }
}
