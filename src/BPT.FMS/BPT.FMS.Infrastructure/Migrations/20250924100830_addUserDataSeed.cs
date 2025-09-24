using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPT.FMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUserDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedBy", "CreatedDate", "Email", "IsActive", "Password", "UserName" },
                values: new object[] { new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"), "Read-Write", new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"), new DateTime(2025, 8, 17, 19, 31, 26, 0, DateTimeKind.Utc), "admin@gmail.com", true, "admin12345", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"));
        }
    }
}
