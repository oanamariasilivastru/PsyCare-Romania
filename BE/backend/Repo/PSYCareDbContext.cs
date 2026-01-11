using Microsoft.EntityFrameworkCore;
using backend.Domain;

namespace backend.Repo
{
    public class PSYCareDbContext : DbContext
    {
        public PSYCareDbContext(DbContextOptions<PSYCareDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Planificator> Planificators { get; set; }
        public DbSet<VaultIdentifier> VaultIdentifiers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Psychologist>().ToTable("Psychologist");
            modelBuilder.Entity<Mood>().ToTable("Mood");
            modelBuilder.Entity<Planificator>().ToTable("Planificator");
            modelBuilder.Entity<VaultIdentifier>().ToTable("VaultIdentifier");
            
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);
            
            modelBuilder.Entity<Psychologist>()
                .HasKey(p => p.Id);
            
            modelBuilder.Entity<Planificator>()
                .HasKey(p => new { p.PsychologistId, p.PatientId, p.Date }); 

            modelBuilder.Entity<Planificator>()
                .HasOne(p => p.Psychologist)
                .WithMany() 
                .HasForeignKey(p => p.PsychologistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Planificator>()
                .HasOne(p => p.Patient)
                .WithMany() 
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Planificator>()
                .Property(p => p.Fee)
                .HasColumnType("decimal(6,2)");
            
            modelBuilder.Entity<Mood>()
                .HasKey(m => new { m.PatientId, m.Date }); 

            modelBuilder.Entity<Mood>()
                .HasOne(m => m.Patient)
                .WithMany() 
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<VaultIdentifier>()
                .HasKey(v => v.Token);

            modelBuilder.Entity<VaultIdentifier>()
                .Property(v => v.DataType)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<VaultIdentifier>()
                .Property(v => v.EncryptedValue)
                .IsRequired();

            modelBuilder.Entity<VaultIdentifier>()
                .Property(v => v.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }
}
