using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskAuthenticationAuthorization.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "BuyerType",
                value: "regular");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "BuyerType",
                value: "regular");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BuyerType", "Email", "Password", "Role" },
                values: new object[] { 3, "golden", "user1@example.com", "user123", "Buyer" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "BuyerType",
                table: "Users");
        }
    }
}
