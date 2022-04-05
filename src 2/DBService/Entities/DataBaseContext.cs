using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DBService.Entities
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
            ConfigureDataBase();
        }

        private void ConfigureDataBase()
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<AuthorBook> AuthorsBooks { get; set; }
        public virtual DbSet<ClientBook> ClientBooks { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("DbContext is not Configured");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey("BookId");

                entity.HasIndex(b => b.Title).IsUnique();

                entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

                entity.Property(e => e.CreateDate)
                .IsRequired();

            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey("AuthorId");

                entity.HasIndex(a => a.Name).IsUnique();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey("ClientId");

                entity.HasIndex(c => c.Name).IsUnique();

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

                //entity.HasMany(c => c.BorrowedBooks)
                //.WithOne(b => b.Client)
                //.HasConstraintName("RelacionClienteLibros")
                //.OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthorBook>(entity =>
            {
                entity.HasKey(all => new { all.BookId, all.AuthorId });
            });

            modelBuilder.Entity<ClientBook>(entity =>
            {
                entity.HasKey(all => new { all.ClientId, all.BookId });
            });

            base.OnModelCreating(modelBuilder);
        }

        
    }
}
