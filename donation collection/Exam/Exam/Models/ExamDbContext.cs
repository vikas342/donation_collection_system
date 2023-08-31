using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Exam.Models
{
    public partial class ExamDbContext : DbContext
    {
        public ExamDbContext()
        {
        }

        public ExamDbContext(DbContextOptions<ExamDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Object> Objects { get; set; } = null!;
        public virtual DbSet<ObjectType> ObjectTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserdeatilSP> UserdeatilSP { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=PC0334\\MSSQL2019;Database=ExamDb;Trusted_Connection=True;TrustServerCertificate=True;");
                optionsBuilder.UseSqlServer("Server=DESKTOP-1CPKOO6\\SQLEXPRESS;Database=ExamDb;Trusted_Connection=True;TrustServerCertificate=True;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Aid)
                    .HasName("PK__Address__C69007C82F89AA01");

                entity.ToTable("Address");

                entity.Property(e => e.Aid).HasColumnName("AID");

                //entity.Property(e => e.Area)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("area");

                //entity.Property(e => e.City)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("city");

                //entity.Property(e => e.Flatno)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Pincode)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.State)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("state");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("FK__Address__UID__30F848ED");
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.ToTable("object");

                entity.Property(e => e.Object1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Object");

                entity.Property(e => e.TypeId).HasColumnName("Type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Objects)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__object__Type_id__2B3F6F97");
            });

            modelBuilder.Entity<ObjectType>(entity =>
            {
                entity.ToTable("object_type");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Pid)
                    .HasName("PK__Payment__C577552080EED5F4");

                entity.ToTable("Payment");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK__Payment__Status__34C8D9D1");

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.Uid)
                    .HasConstraintName("FK__Payment__UID__33D4B598");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Role1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Createddate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsUnicode(false);

                entity.Property(e => e.PasswordSalt).IsUnicode(false);

                entity.Property(e => e.Photo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UID");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__Users__Role__2E1BDC42");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
