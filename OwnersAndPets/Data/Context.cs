using System.Data.Entity;
using OwnersAndPets.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace OwnersAndPets.Data
{
    public sealed class Context : DbContext
    {

        public static Context Create()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            return new Context(connectionString);
        }

        private Context(string connectionString): base(connectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>().HasKey(t => t.Id).Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Owner>().HasKey(t => t.Id).Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<OwnerPet>().HasKey(t => new
            {
                t.OwnerId,
                t.PetId
            });
        }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<OwnerPet> OwnerPets { get; set; }
    }
}