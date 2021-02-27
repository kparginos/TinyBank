using Microsoft.EntityFrameworkCore;

using TinyBank.Model;

namespace TinyBank.Data
{
    public class TinyBankDBContext : DbContext
    {
        public DbSet<Customer> Customer { get; private set; }
        public DbSet<Accounts> Accounts { get; private set; }
        public DbSet<CustomerAccounts_V> CustomerAccountsView { get; private set; }

        public TinyBankDBContext(DbContextOptions<TinyBankDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer Table Entity
            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

            // CustomerAccounts_V View Entity
            modelBuilder.Entity<CustomerAccounts_V>()
                .ToView("CustomerAccounts_V")
                .HasNoKey();

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
