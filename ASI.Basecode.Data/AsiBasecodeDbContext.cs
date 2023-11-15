using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDBContext : DbContext
    {
        public AsiBasecodeDBContext()
        {
        }

        public AsiBasecodeDBContext(DbContextOptions<AsiBasecodeDBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BookMaster> BookMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__1788CC4D5F4A160F")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookMaster>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.HasIndex(e => e.BId, "UQ__Users__1788CC4D5F4A160F");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookAuthor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookDes)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BookImage)
                       .IsRequired()
                       .HasColumnType("varchar(max)");

                entity.Property(e => e.BookAdded).HasColumnType("datetime");

                entity.Property(e => e.BId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
       
    }
}
