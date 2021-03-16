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
            modelBuilder.Entity<Customer>(
                builder =>
                {
                    builder.ToTable("Customer");
                    builder.HasIndex(c => c.VatNumber)
                                    .IsUnique();
                    builder.HasIndex(c => c.CustBankID)
                                    .IsUnique();
                    builder.OwnsOne<AuditInfo>(c => c.AuditInfo);
                });

            // CustomerAccounts_V View Entity
            modelBuilder.Entity<CustomerAccounts_V>(
                builder =>
                {
                    builder.ToView("CustomerAccounts_V");
                    builder.HasNoKey();
                });

            // Accounts Table Entity
            modelBuilder.Entity<Accounts>(
                builder =>
                {
                    builder.ToTable("Accounts");
                    builder
                    .HasIndex(an => an.AccountNumber)
                    .IsUnique();

                    builder.OwnsOne<AuditInfo>(a => a.AuditInfo);
                });

            // Transaction Table Entity
            modelBuilder.Entity<Transaction>(
                builder =>
                {
                    builder.ToTable("Transaction");
                });                

            // Cards Table Entity
            modelBuilder.Entity<Card>(
                builder =>
                {
                    builder.ToTable("Card");
                    builder
                    .HasIndex(c => c.CardNumber)
                    .IsUnique();
                });
                
        }
    }
}
