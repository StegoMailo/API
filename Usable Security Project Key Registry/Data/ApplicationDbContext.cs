using Microsoft.EntityFrameworkCore;
using Usable_Security_Project_Key_Registry.Models;

namespace Usable_Security_Project_Key_Registry.Data
{
    public class ApplicationDbContext : DbContext 
    { 
    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> User {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) //Fluent API
        {
            modelBuilder.Entity<User>()
                .Property(s => s.Email)
                .HasColumnName("Email")
                .IsRequired();

        }


    }
}
