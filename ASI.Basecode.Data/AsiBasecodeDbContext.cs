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
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserId, "UQ__Users__1788CC4D5F4A160F")
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

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                
                entity.HasIndex(e => e.BookId, "UQ__Users__1788CC4D5F4A160F")
                    .IsUnique(); ; 

                entity.Property(e => e.BookTitle)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.BookGenre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BookAuthor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BookSeries)
                    .HasMaxLength(50);

                entity.Property(e => e.BookDescription)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime).HasColumnType("datetime");

              });

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
       
    }
}
