using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lab_DB_IndEx_MIA
{
    public partial class MIAContext : DbContext
    {
        public MIAContext()
        {
        }

        public MIAContext(DbContextOptions<MIAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Criminal> Criminals { get; set; }
        public virtual DbSet<Empolyee> Empolyees { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<TypesCrime> TypesCrimes { get; set; }
        public virtual DbSet<Victim> Victims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\АШПИ\\Desktop\\БД\\MIA.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Criminal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.FullName).HasColumnName("fullName");

                entity.Property(e => e.IdEmpolyees)
                    .HasColumnType("int")
                    .HasColumnName("id_Empolyees");

                entity.Property(e => e.IdTypeofCrime)
                    .HasColumnType("int")
                    .HasColumnName("id_typeofCrime");

                entity.Property(e => e.IdVictim)
                    .HasColumnType("int")
                    .HasColumnName("id_victim");

                entity.Property(e => e.Male).HasColumnName("male");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.IdEmpolyeesNavigation)
                    .WithMany(p => p.Criminals)
                    .HasForeignKey(d => d.IdEmpolyees);

                entity.HasOne(d => d.IdTypeofCrimeNavigation)
                    .WithMany(p => p.Criminals)
                    .HasForeignKey(d => d.IdTypeofCrime);

                entity.HasOne(d => d.IdVictimNavigation)
                    .WithMany(p => p.Criminals)
                    .HasForeignKey(d => d.IdVictim);
            });

            modelBuilder.Entity<Empolyee>(entity =>
            {
                entity.HasIndex(e => e.Pasport, "IX_Empolyees_pasport")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Age)
                    .HasColumnType("int")
                    .HasColumnName("age");

                entity.Property(e => e.IdPost)
                    .HasColumnType("int")
                    .HasColumnName("id_post");

                entity.Property(e => e.IdTitle)
                    .HasColumnType("int")
                    .HasColumnName("id_title");

                entity.Property(e => e.Male).HasColumnName("male");

                entity.Property(e => e.Pasport).HasColumnName("pasport");

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasColumnName("telephone");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Empolyees)
                    .HasForeignKey(d => d.IdPost);

                entity.HasOne(d => d.IdTitleNavigation)
                    .WithMany(p => p.Empolyees)
                    .HasForeignKey(d => d.IdTitle);

                entity.Property(e => e.dateTime).HasColumnName("DateOfBirth");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.Property(e => e.IdPost).HasColumnName("id_post");

                entity.Property(e => e.NamePost).HasColumnName("Name_Post");
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.HasKey(e => e.IdTitle);

                entity.Property(e => e.IdTitle).HasColumnName("id_title");

                entity.Property(e => e.NameTitle).HasColumnName("Name_Title");
            });

            modelBuilder.Entity<TypesCrime>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Victim>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
