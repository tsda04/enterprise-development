using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace BikeRental.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BikeModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<int>(type: "int", nullable: false),
                    wheel_size = table.Column<int>(type: "int", nullable: false),
                    max_cyclist_weight = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<double>(type: "double", nullable: false),
                    brake_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    year_of_manufacture = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    rent_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bike_models", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Renters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_renters", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    serial_number = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    color = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bikes", x => x.id);
                    table.ForeignKey(
                        name: "fk_bikes_bike_models_model_id",
                        column: x => x.model_id,
                        principalTable: "BikeModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    bike_id = table.Column<int>(type: "int", nullable: false),
                    rental_start_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    rental_duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_leases", x => x.id);
                    table.ForeignKey(
                        name: "fk_leases_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "Bikes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_leases_renters_renter_id",
                        column: x => x.renter_id,
                        principalTable: "Renters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_bike_models_type",
                table: "BikeModels",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_bike_models_type_wheel_size",
                table: "BikeModels",
                columns: new[] { "type", "wheel_size" });

            migrationBuilder.CreateIndex(
                name: "ix_bikes_model_id",
                table: "Bikes",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_bikes_serial_number",
                table: "Bikes",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_leases_bike_id",
                table: "Leases",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "ix_leases_renter_id",
                table: "Leases",
                column: "renter_id");

            migrationBuilder.CreateIndex(
                name: "ix_renters_phone_number",
                table: "Renters",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "Renters");

            migrationBuilder.DropTable(
                name: "BikeModels");
        }
    }
}
