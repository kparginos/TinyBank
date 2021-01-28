﻿using Microsoft.EntityFrameworkCore;
using TinyBank.Core.Model;

namespace TinyBank.Core.Data
{
    public class TinyBankDBContext : DbContext
    {
        const string connString = "Server=localhost,1434;Database=tinybank;User Id=sa;Password=admin!@#123";
        public TinyBankDBContext()
        {

        }
        public TinyBankDBContext(DbContextOptions<TinyBankDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connString,
                options =>
                {
                    options.MigrationsAssembly("TinyBank");
                });
            //optionsBuilder.UseSqlServer(connString);
        }

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
