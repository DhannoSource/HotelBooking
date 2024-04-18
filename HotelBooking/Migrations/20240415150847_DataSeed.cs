using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_Address_AddressId",
                table: "Hotel");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "AddreeLine1", "AddressLine2", "City", "Country", "Phone", "PostalCode", "State" },
                values: new object[,]
                {
                    { 1, "Seminyak", "Bali", "Bali", "Indonesia", null, "72349", null },
                    { 2, "Nariman Point", "South Mumbai", "Mumbai", "India", null, "4473836", null },
                    { 3, "Orchard Rd", "Sommerset", "Singapore", "Singapore", null, "633578", null }
                });

            migrationBuilder.InsertData(
                table: "Hotel",
                columns: new[] { "Id", "AddressId", "Description", "HasGym", "HasPool", "Name", "NoOfRooms", "Stars", "UserRating" },
                values: new object[,]
                {
                    { 1, 1, null, true, true, "Mariott", 30, 5, 5 },
                    { 2, 2, null, true, true, "Hilton", 30, 5, 5 },
                    { 3, 3, null, true, true, "Pan Pacific", 30, 5, 5 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_Address_AddressId",
                table: "Hotel",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_Address_AddressId",
                table: "Hotel");

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Hotel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_Address_AddressId",
                table: "Hotel",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
