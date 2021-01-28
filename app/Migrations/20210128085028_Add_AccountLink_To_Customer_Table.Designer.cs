﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TinyBank.Core.Data;

namespace TinyBank.Migrations
{
    [DbContext(typeof(TinyBankDBContext))]
    [Migration("20210128085028_Add_AccountLink_To_Customer_Table")]
    partial class Add_AccountLink_To_Customer_Table
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

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerId")
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

            modelBuilder.Entity("TinyBank.Core.Model.Accounts", b =>
                {
                    b.HasOne("TinyBank.Core.Model.Customer", null)
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("TinyBank.Core.Model.Customer", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
