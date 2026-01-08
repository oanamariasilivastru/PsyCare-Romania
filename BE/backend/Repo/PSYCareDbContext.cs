using System.Data.Entity;
using backend.Domain;

namespace backend.Repo
{
    public class PSYCareDbContext : DbContext
    {
        // Use the connection string name from App.config
        public PSYCareDbContext() : base("Server=DESKTOP-29QIGKD;Database=PSYCare;Trusted_Connection=True;Encrypt=False;") {}

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map tables explicitly (optional, but clearer)
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Psychologist>().ToTable("Psychologist");
        }
    }
}