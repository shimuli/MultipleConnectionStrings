using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MultipleConnectionStrings.Models.Users
{
    public partial class MultipleConnectionStringContext : DbContext
    {

        public MultipleConnectionStringContext(DbContextOptions<MultipleConnectionStringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConnectionTable> ConnectionTables { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=UsersConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ConnectionTable>(entity =>
            {
                entity.ToTable("ConnectionTable");

                entity.Property(e => e.ConnectionName).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.DatePosted).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.HasOne(d => d.Connection)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ConnectionId)
                    .HasConstraintName("FK__Users__Connectio__25869641");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
