using Microsoft.EntityFrameworkCore;
using TinyBank.Core.Model;

namespace TinyBank.Core.Data
{
    public class TinyBankDBContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.VatNumber)
                .IsUnique();
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.CustBankID)
                .IsUnique();

            //modelBuilder.Entity<Accounts>()
            //    .ToTable("Order");
        }
    }
}
