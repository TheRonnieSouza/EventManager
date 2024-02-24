using DevEvents.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevEvents.API.Persistence
{
    public class EventManagerDbContext : DbContext
    {
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options) 
        {

        }        
        public DbSet<EventManager> EventManager { get; set; }
        public DbSet<EventManagerSpeaker> EventManagerSpeaker { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {               
            builder.Entity<EventManager>(
                e =>
                {
                    e.HasKey(de => de.Id);
                    e.Property(i => i.Id).HasColumnName("ID");

                    e.Property(de => de.Title)
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("TITLE")
                    .IsRequired();

                    e.Property(de => de.Description)
                    .HasMaxLength(300)
                    .HasColumnType("VARCHAR(300)")
                    .HasColumnName("DESCRIPTION")
                    .IsRequired(false);

                    e.Property(de => de.StartDate)
                    .HasColumnType("DATETIME")
                    .HasColumnName("START_DATE");

                    e.Property(de => de.EndDate)
                    .HasColumnType("DATETIME")
                    .HasColumnName("END_DATE");

                    e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(de => de.EventManagerId);
                });

            builder.Entity<EventManagerSpeaker>(
           e =>
           {
               e.HasKey(de => de.Id);
               e.Property(i => i.Id).HasColumnName("ID");
           });

        }
    }
}
