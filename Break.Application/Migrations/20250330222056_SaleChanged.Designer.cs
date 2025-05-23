﻿// <auto-generated />
using System;
using Break.Application.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Break.Application.Migrations
{
    [DbContext(typeof(BreakAppDbContext))]
    [Migration("20250330222056_SaleChanged")]
    partial class SaleChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Break.Application.Models.AppliedOffer", b =>
                {
                    b.Property<int>("AppliedOfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AppliedOfferId"));

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.Property<int>("SaleId")
                        .HasColumnType("integer");

                    b.HasKey("AppliedOfferId");

                    b.HasIndex("OfferId");

                    b.HasIndex("SaleId");

                    b.ToTable("AppliedOffer");
                });

            modelBuilder.Entity("Break.Application.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemId"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MaximumStockLevel")
                        .HasColumnType("integer");

                    b.Property<int>("MinimumStockLevel")
                        .HasColumnType("integer");

                    b.Property<string>("ProductCategory")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("QuantityInStock")
                        .HasColumnType("integer");

                    b.Property<int>("ReorderQuantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("ItemId");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("ProductCode")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Break.Application.Models.Offer", b =>
                {
                    b.Property<int>("OfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OfferId"));

                    b.Property<string>("CouponCode")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("DiscountType")
                        .HasColumnType("integer");

                    b.Property<decimal>("DiscountValue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("OfferDescription")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("OfferName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("OfferId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("Break.Application.Models.OfferCondition", b =>
                {
                    b.Property<int>("OfferConditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OfferConditionId"));

                    b.Property<int>("ConditionType")
                        .HasColumnType("integer");

                    b.Property<int?>("ItemId")
                        .HasColumnType("integer");

                    b.Property<decimal?>("MaximumAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int?>("MaximumQuantity")
                        .HasColumnType("integer");

                    b.Property<decimal?>("MinimumAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int?>("MinimumQuantity")
                        .HasColumnType("integer");

                    b.Property<int>("OfferId")
                        .HasColumnType("integer");

                    b.HasKey("OfferConditionId");

                    b.HasIndex("ItemId");

                    b.HasIndex("OfferId");

                    b.ToTable("OfferConditions");
                });

            modelBuilder.Entity("Break.Application.Models.Sale", b =>
                {
                    b.Property<int>("SaleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SaleId"));

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("SaleId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Break.Application.Models.SaleItem", b =>
                {
                    b.Property<int>("SaleItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SaleItemId"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<decimal>("LineTotal")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("SaleId")
                        .HasColumnType("integer");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("SaleItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleItems");
                });

            modelBuilder.Entity("Break.Application.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ItemOffer", b =>
                {
                    b.Property<int>("ItemsItemId")
                        .HasColumnType("integer");

                    b.Property<int>("OffersOfferId")
                        .HasColumnType("integer");

                    b.HasKey("ItemsItemId", "OffersOfferId");

                    b.HasIndex("OffersOfferId");

                    b.ToTable("OfferItems", (string)null);
                });

            modelBuilder.Entity("Break.Application.Models.AppliedOffer", b =>
                {
                    b.HasOne("Break.Application.Models.Offer", "Offer")
                        .WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Break.Application.Models.Sale", "Sale")
                        .WithMany("AppliedOffers")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Break.Application.Models.OfferCondition", b =>
                {
                    b.HasOne("Break.Application.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Break.Application.Models.Offer", "Offer")
                        .WithMany("OfferConditions")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("Break.Application.Models.SaleItem", b =>
                {
                    b.HasOne("Break.Application.Models.Item", "Item")
                        .WithMany("SaleItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Break.Application.Models.Sale", "Sale")
                        .WithMany("SaleItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("ItemOffer", b =>
                {
                    b.HasOne("Break.Application.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Break.Application.Models.Offer", null)
                        .WithMany()
                        .HasForeignKey("OffersOfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Break.Application.Models.Item", b =>
                {
                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("Break.Application.Models.Offer", b =>
                {
                    b.Navigation("OfferConditions");
                });

            modelBuilder.Entity("Break.Application.Models.Sale", b =>
                {
                    b.Navigation("AppliedOffers");

                    b.Navigation("SaleItems");
                });
#pragma warning restore 612, 618
        }
    }
}
