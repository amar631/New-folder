using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace CureHospitalDALCrossPlatform.Models
{
    public partial class CureHospitalDBContext : DbContext
    {
        public CureHospitalDBContext()
        {
        }

        public CureHospitalDBContext(DbContextOptions<CureHospitalDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DoctorSpecialization> DoctorSpecialization { get; set; }
        public virtual DbSet<Specialization> Specialization { get; set; }
        public virtual DbSet<Surgery> Surgery { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("CureHospitalDBConnectionString");
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(connectionString);
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DoctorSpecialization>(entity =>
            {
                entity.HasKey(e => new { e.DoctorId, e.SpecializationCode })
                    .HasName("pk_DoctorIDSpecializatioinCode");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.SpecializationCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SpecializationDate).HasColumnType("date");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorSpecialization)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DoctorID");

                entity.HasOne(d => d.SpecializationCodeNavigation)
                    .WithMany(p => p.DoctorSpecialization)
                    .HasForeignKey(d => d.SpecializationCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SpecializatioinCode");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.HasKey(e => e.SpecializationCode)
                    .HasName("pk_SpecializationCode");

                entity.Property(e => e.SpecializationCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SpecializationName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Surgery>(entity =>
            {
                entity.Property(e => e.SurgeryId).HasColumnName("SurgeryID");

                entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

                entity.Property(e => e.EndTime).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.StartTime).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.SurgeryCategory)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SurgeryDate).HasColumnType("date");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Surgery)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("fk_DoctorID_Surgery");

                entity.HasOne(d => d.SurgeryCategoryNavigation)
                    .WithMany(p => p.Surgery)
                    .HasForeignKey(d => d.SurgeryCategory)
                    .HasConstraintName("fk_SpecializatioinCode_Surgery");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
