using Microsoft.EntityFrameworkCore;
using backend.Domain;

namespace backend.Repo
{
    public class PSYCareDbContext : DbContext
    {
        public PSYCareDbContext(DbContextOptions<PSYCareDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Planificator> Planificators { get; set; }
        public DbSet<VaultIdentifier> VaultIdentifiers { get; set; }
        
        // NEW: Add these DbSets
        public DbSet<Message> Messages { get; set; }
        public DbSet<TestCompletion> TestCompletions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Psychologist>().ToTable("Psychologist");
            modelBuilder.Entity<Mood>().ToTable("Mood");
            modelBuilder.Entity<Planificator>().ToTable("Planificator");
            modelBuilder.Entity<VaultIdentifier>().ToTable("VaultIdentifier");
            
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<TestCompletion>().ToTable("TestCompletion");

            modelBuilder.Entity<Patient>().HasKey(p => p.Id);
            modelBuilder.Entity<Psychologist>().HasKey(p => p.Id);

            modelBuilder.Entity<Planificator>().HasKey(p => new { p.PsychologistId, p.PatientId, p.Date });
            modelBuilder.Entity<Planificator>()
                .HasOne(p => p.Psychologist)
                .WithMany()
                .HasForeignKey(p => p.PsychologistId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Planificator>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Planificari)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Planificator>().Property(p => p.Fee).HasColumnType("decimal(6,2)");

            modelBuilder.Entity<Mood>().HasKey(m => new { m.PatientId, m.Date });
            modelBuilder.Entity<Mood>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Moods)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VaultIdentifier>().HasKey(v => v.Token);
            modelBuilder.Entity<VaultIdentifier>().Property(v => v.DataType).HasMaxLength(30).IsRequired();
            modelBuilder.Entity<VaultIdentifier>().Property(v => v.EncryptedValue).IsRequired();
            modelBuilder.Entity<VaultIdentifier>().Property(v => v.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            
            modelBuilder.Entity<Message>().HasKey(m => m.Id);
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.SenderId, m.ReceiverId, m.SentAt })
                .HasDatabaseName("idx_conversation");
            modelBuilder.Entity<Message>()
                .HasIndex(m => new { m.ReceiverId, m.IsRead })
                .HasDatabaseName("idx_unread");
            modelBuilder.Entity<Message>().Property(m => m.SenderType).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Message>().Property(m => m.ReceiverType).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<Message>().Property(m => m.Content).IsRequired();
            modelBuilder.Entity<Message>().Property(m => m.SentAt).HasDefaultValueSql("SYSUTCDATETIME()");
            modelBuilder.Entity<Message>().Property(m => m.IsRead).HasDefaultValue(false);
            
            modelBuilder.Entity<TestCompletion>().HasKey(tc => tc.Id);
            modelBuilder.Entity<TestCompletion>()
                .HasIndex(tc => new { tc.PatientId, tc.CompletedAt })
                .HasDatabaseName("idx_patient_tests")
                .IsDescending(false, true);
            modelBuilder.Entity<TestCompletion>()
                .HasOne(tc => tc.Patient)
                .WithMany()
                .HasForeignKey(tc => tc.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TestCompletion>().Property(tc => tc.TestCode).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<TestCompletion>().Property(tc => tc.TotalScore).IsRequired();
            modelBuilder.Entity<TestCompletion>().Property(tc => tc.ResultLabel).HasMaxLength(100);
            modelBuilder.Entity<TestCompletion>().Property(tc => tc.Severity).HasMaxLength(20);
            modelBuilder.Entity<TestCompletion>().Property(tc => tc.CompletedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }
}