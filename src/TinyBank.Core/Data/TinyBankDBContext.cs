using Microsoft.EntityFrameworkCore;
using TinyBank.Core.Model;

namespace TinyBank.Core.Data
{
    public class TinyBankDBContext : DbContext
    {        
        public TinyBankDBContext(DbContextOptions<TinyBankDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer Table Entity
            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.VatNumber)
                .IsUnique();
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustBankID)
                .IsUnique();

            // Accounts Table Entity
            modelBuilder.Entity<Accounts>()
                .ToTable("Accounts");
            modelBuilder.Entity<Accounts>()
                .HasIndex(an => an.AccountNumber)
                .IsUnique();

            // Transaction Table Entity
            modelBuilder.Entity<Transaction>()
                .ToTable("Transaction");

        }
    }
}
