using Dtos.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DBService.Entities
{
    public class InvoicesDbContext : DbContext
    {
        public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options): base(options)
        {
            ConfigureDataBase();
        }

        private void ConfigureDataBase()
        {
            Database.EnsureCreated();//Create the db if the file doesnt exists
            if (this.TransactionStatus.Count() == 0)    //If the db dont have the status list
            {
                var statusList = Enum.GetValues(typeof(EnumStatus)) //Get the status list with the enum values and save in to the db
                .Cast<EnumStatus>()
                .Select(e => new TransactionStatus { StatusId = (int)e, Description = e.ToString() })
                .ToList();
                this.TransactionStatus.AddRange(statusList);
                this.SaveChanges();
            }
            
        }

        public virtual DbSet<TransactionStatus> TransactionStatus { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<TransactionHistoric> TransactionHistorics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("DbContext is not Configured");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.StatusId)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("getdate()")
                    //.HasDefaultValue(DateTime.Now)
                    .IsRequired();

                entity.Property(e => e.Amount)
                    .IsRequired();

                entity.Property(e => e.LastUpdatedDate)
                    //.HasDefaultValue(DateTime.Now)
                    .HasDefaultValueSql("getdate()")
                    .IsRequired();

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Transactions_Status");
            });

            modelBuilder.Entity<TransactionHistoric>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date)
                    //.HasDefaultValue(DateTime.Now)
                    .HasDefaultValueSql("getdate()");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Historic)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_TransactionHistoric_Transaction");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TransactionHistorics)
                    .HasForeignKey(d => d.NewStatusId)
                    .HasConstraintName("FK_TransactionHistorics_Status_NewStatusId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TransactionHistorics)
                    .HasForeignKey(d => d.OldStatusId)
                    .HasConstraintName("FK_TransactionHistorics_Status_OldStatusId");
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
