﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TinyBank.Data;

namespace TinyBank.Migrations
{
    [DbContext(typeof(TinyBankDBContext))]
    [Migration("20210213160332_Rename_Account_Column_Active_to_State")]
    partial class Rename_Account_Column_Active_to_State
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("TinyBank.Core.Model.Accounts", b =>
                {
                    b.Property<int>("AccountsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AccountDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("AccountsId");

                    b.HasIndex("AccountNumber")
                        .IsUnique()
                        .HasFilter("[AccountNumber] IS NOT NULL");

                    b.HasIndex("CustomerId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustBankID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CustType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SureName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatNumber")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CustomerId");

                    b.HasIndex("CustBankID")
                        .IsUnique()
                        .HasFilter("[CustBankID] IS NOT NULL");

                    b.HasIndex("VatNumber")
                        .IsUnique()
                        .HasFilter("[VatNumber] IS NOT NULL");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("AccountsId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("TransDescr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountsId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Accounts", b =>
                {
                    b.HasOne("TinyBank.Core.Model.Customer", null)
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Transaction", b =>
                {
                    b.HasOne("TinyBank.Core.Model.Accounts", null)
                        .WithMany("Transactions")
                        .HasForeignKey("AccountsId");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Accounts", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Customer", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
