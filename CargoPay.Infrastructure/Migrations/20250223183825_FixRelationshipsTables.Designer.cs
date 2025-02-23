﻿// <auto-generated />
using System;
using CargoPay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CargoPay.Infrastructure.Migrations
{
    [DbContext(typeof(CargoPayDbContext))]
    [Migration("20250223183825_FixRelationshipsTables")]
    partial class FixRelationshipsTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CargoPay.Core.Entitites.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("CardId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Fee")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.Card", b =>
                {
                    b.HasOne("CargoPay.Core.Entitites.User", "User")
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.Payment", b =>
                {
                    b.HasOne("CargoPay.Core.Entitites.Card", "Card")
                        .WithMany("Payments")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CargoPay.Core.Entitites.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.Card", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("CargoPay.Core.Entitites.User", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
