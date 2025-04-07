using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pv311_web_api.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CarsandManufacturestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufactureId",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Manufactures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Founder = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Director = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufactures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ManufactureId",
                table: "Cars",
                column: "ManufactureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars",
                column: "ManufactureId",
                principalTable: "Manufactures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Manufactures_ManufactureId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Manufactures");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ManufactureId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ManufactureId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");
        }
    }
}