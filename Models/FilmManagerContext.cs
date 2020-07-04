using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FilmManagement_BE.Models
{
    public partial class FilmManagerContext : DbContext
    {
        public FilmManagerContext()
        {
        }

        public FilmManagerContext(DbContextOptions<FilmManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentImage> EquipmentImage { get; set; }
        public virtual DbSet<Scenario> Scenario { get; set; }
        public virtual DbSet<ScenarioAccountDetail> ScenarioAccountDetail { get; set; }
        public virtual DbSet<ScenarioEquipmentDetail> ScenarioEquipmentDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceToken).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateById).HasColumnName("CreateByID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.CreateBy)
                    .WithMany(p => p.EquipmentCreateBy)
                    .HasForeignKey(d => d.CreateById)
                    .HasConstraintName("FK_Equipment_Account");

                entity.HasOne(d => d.LastModifiedBy)
                    .WithMany(p => p.EquipmentLastModifiedBy)
                    .HasForeignKey(d => d.LastModifiedById)
                    .HasConstraintName("FK_Equipment_Account1");
            });

            modelBuilder.Entity<EquipmentImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.Url).IsUnicode(false);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.EquipmentImage)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK_EquipmentImage_Equipment");
            });

            modelBuilder.Entity<Scenario>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateById).HasColumnName("CreateByID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.Property(e => e.Location).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Script).IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("datetime");

                entity.Property(e => e.TimeStart).HasColumnType("datetime");

                entity.HasOne(d => d.CreateBy)
                    .WithMany(p => p.ScenarioCreateBy)
                    .HasForeignKey(d => d.CreateById)
                    .HasConstraintName("FK_Scenario_Account");

                entity.HasOne(d => d.LastModifiedBy)
                    .WithMany(p => p.ScenarioLastModifiedBy)
                    .HasForeignKey(d => d.LastModifiedById)
                    .HasConstraintName("FK_Scenario_Account1");
            });

            modelBuilder.Entity<ScenarioAccountDetail>(entity =>
            {
                entity.HasKey(e => new { e.ScenarioId, e.AccountId });

                entity.Property(e => e.ScenarioId).HasColumnName("ScenarioID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Characters).HasMaxLength(1000);

                entity.Property(e => e.CreateById).HasColumnName("CreateByID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ScenarioAccountDetailAccount)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScenarioAccountDetail_Account");

                entity.HasOne(d => d.CreateBy)
                    .WithMany(p => p.ScenarioAccountDetailCreateBy)
                    .HasForeignKey(d => d.CreateById)
                    .HasConstraintName("FK_ScenarioAccountDetail_Account_3");

                entity.HasOne(d => d.LastModifiedBy)
                    .WithMany(p => p.ScenarioAccountDetailLastModifiedBy)
                    .HasForeignKey(d => d.LastModifiedById)
                    .HasConstraintName("FK_ScenarioAccountDetail_Account2");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.ScenarioAccountDetail)
                    .HasForeignKey(d => d.ScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScenarioAccountDetail_Scenario");
            });

            modelBuilder.Entity<ScenarioEquipmentDetail>(entity =>
            {
                entity.HasKey(e => new { e.ScenarioId, e.EquipmentId });

                entity.Property(e => e.ScenarioId).HasColumnName("ScenarioID");

                entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");

                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedById).HasColumnName("LastModifiedByID");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.ScenarioEquipmentDetailCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_ScenarioEquipmentDetail_Account");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.ScenarioEquipmentDetail)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScenarioEquipmentDetail_Equipment");

                entity.HasOne(d => d.LastModifiedBy)
                    .WithMany(p => p.ScenarioEquipmentDetailLastModifiedBy)
                    .HasForeignKey(d => d.LastModifiedById)
                    .HasConstraintName("FK_ScenarioEquipmentDetail_Account1");

                entity.HasOne(d => d.Scenario)
                    .WithMany(p => p.ScenarioEquipmentDetail)
                    .HasForeignKey(d => d.ScenarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScenarioEquipmentDetail_Scenario");
            });
        }
    }
}
