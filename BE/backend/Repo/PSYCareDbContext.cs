using Microsoft.EntityFrameworkCore;
using backend.Domain;

namespace backend.Repo
{
    public class PSYCareDbContext : DbContext
    {
        public PSYCareDbContext(DbContextOptions<PSYCareDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Psychologist>().ToTable("Psychologist");
        }
    }
}