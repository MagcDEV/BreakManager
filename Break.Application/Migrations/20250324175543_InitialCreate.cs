using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Break.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ProductCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReorderQuantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    QuantityInStock = table.Column<int>(type: "integer", nullable: false),
                    MinimumStockLevel = table.Column<int>(type: "integer", nullable: false),
                    MaximumStockLevel = table.Column<int>(type: "integer", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OfferDescription = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CouponCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                });

            migrationBuilder.CreateTable(
                name: "OfferConditions",
                columns: table => new
                {
                    OfferConditionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    ConditionType = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "integer", nullable: true),
                    MaximumQuantity = table.Column<int>(type: "integer", nullable: true),
                    MinimumAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    MaximumAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferConditions", x => x.OfferConditionId);
                    table.ForeignKey(
                        name: "FK_OfferConditions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfferConditions_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferItems",
                columns: table => new
                {
                    ItemsItemId = table.Column<int>(type: "integer", nullable: false),
                    OffersOfferId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferItems", x => new { x.ItemsItemId, x.OffersOfferId });
                    table.ForeignKey(
                        name: "FK_OfferItems_Items_ItemsItemId",
                        column: x => x.ItemsItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferItems_Offers_OffersOfferId",
                        column: x => x.OffersOfferId,
                        principalTable: "Offers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppliedOffer",
                columns: table => new
                {
                    AppliedOfferId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<int>(type: "integer", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedOffer", x => x.AppliedOfferId);
                    table.ForeignKey(
                        name: "FK_AppliedOffer_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedOffer_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    SaleItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SaleId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.SaleItemId);
                    table.ForeignKey(
                        name: "FK_SaleItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppliedOffer_OfferId",
                table: "AppliedOffer",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedOffer_SaleId",
                table: "AppliedOffer",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Barcode",
                table: "Items",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductCode",
                table: "Items",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferConditions_ItemId",
                table: "OfferConditions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferConditions_OfferId",
                table: "OfferConditions",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_OffersOfferId",
                table: "OfferItems",
                column: "OffersOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ItemId",
                table: "SaleItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedOffer");

            migrationBuilder.DropTable(
                name: "OfferConditions");

            migrationBuilder.DropTable(
                name: "OfferItems");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
