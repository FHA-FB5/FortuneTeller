using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Gruppenverteilung.Models
{
    public partial class StudentDistributorDbContext : DbContext
    {
        public StudentDistributorDbContext()
        {
        }

        public StudentDistributorDbContext(DbContextOptions<StudentDistributorDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblGroup> TblGroup { get; set; }
        public virtual DbSet<TblGroupMember> TblGroupMember { get; set; }
        public virtual DbSet<TblMember> TblMember { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:dkoob.database.windows.net,1433;Initial Catalog=StudentDistributorDb;Persist Security Info=False;User ID=Dkoob;Password=Schach100Wasserball;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("tbl_Group");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RoomName).HasMaxLength(10);
            });

            modelBuilder.Entity<TblGroupMember>(entity =>
            {
                entity.HasKey(e => e.GroupMemberId);

                entity.ToTable("tbl_GroupMember");
            });

            modelBuilder.Entity<TblMember>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("tbl_Member");

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
