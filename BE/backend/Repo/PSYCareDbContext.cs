using Microsoft.EntityFrameworkCore;
using backend.Domain;

namespace backend.Repo
{
    public class PSYCareDbContext : DbContext
    {
        public PSYCareDbContext() : base() { }

        public PSYCareDbContext(DbContextOptions<PSYCareDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=DESKTOP-URV53RQ\SQLEXPRESS;Database=PSYCare;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
                );
            }
        }

        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Psychologist> Psychologists { get; set; } = null!;
        public DbSet<VaultIdentifier> VaultIdentifiers { get; set; } = null!;
        public DbSet<Mood> Moods { get; set; } = null!;
        public DbSet<Planificator> Planificators { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("VARCHAR(100)");
                entity.Property(e => e.Password).HasColumnType("VARCHAR(128)");
                entity.Property(e => e.Salt).HasColumnType("VARCHAR(64)");
            });

            // Psychologist
            modelBuilder.Entity<Psychologist>(entity =>
            {
                entity.ToTable("Psychologist");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("VARCHAR(100)");
                entity.Property(e => e.Password).HasColumnType("VARCHAR(128)");
                entity.Property(e => e.Salt).HasColumnType("VARCHAR(64)");
            });

            // VaultIdentifier
            modelBuilder.Entity<VaultIdentifier>(entity =>
            {
                entity.ToTable("VaultIdentifier");
                entity.HasKey(e => e.Token);
                entity.Property(e => e.DataType).HasColumnType("VARCHAR(30)");
                entity.Property(e => e.RetentionUntil).HasColumnType("DATE");
                entity.Property(e => e.CreatedAt).HasColumnType("DATETIME2").HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // Mood - cheie compusă
            modelBuilder.Entity<Mood>(entity =>
            {
                entity.ToTable("Mood");
                entity.HasKey(m => new { m.PatientId, m.Date });

                entity.HasOne(m => m.Patient)
                      .WithMany()
                      .HasForeignKey(m => m.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(m => m.Date).HasDefaultValueSql("GETDATE()");
                entity.Property(m => m.Score).HasDefaultValue((byte)1);
            });

            // Planificator - cheie compusă
            modelBuilder.Entity<Planificator>(entity =>
            {
                entity.ToTable("Planificator");
                entity.HasKey(p => new { p.PsychologistId, p.PatientId, p.Date });

                entity.HasOne(p => p.Psychologist)
                      .WithMany()
                      .HasForeignKey(p => p.PsychologistId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Patient)
                      .WithMany()
                      .HasForeignKey(p => p.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Fee).HasDefaultValue(0.00m);
            });
        }
    }
}