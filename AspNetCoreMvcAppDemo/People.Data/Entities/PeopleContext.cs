using Microsoft.EntityFrameworkCore;
namespace People.Data.Entities
{
    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options) : base(options)
        { }

        public DbSet<Person> People { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetType> PetTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                        .HasKey(p => p.PersonId);

            modelBuilder.Entity<Pet>()
                        .HasAlternateKey(p => p.Name);

            modelBuilder.Entity<PersonPet>()
                        .HasKey(t => new { t.PersonId, t.PetId });

            modelBuilder.Entity<PetType>()
                        .HasAlternateKey(p => p.PetTypeName);
        }
    }
}
