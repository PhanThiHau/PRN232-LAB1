using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232.LMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSeedPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$KfhLqh6JLIldisT5u4ExOOu5umy9Xxeqmr5hBWkj84dt8nyKjUg5a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$KfhLqh6JLIldisT5u4ExOOu5umy9Xxeqmr5hBWkj84dt8nyKjUg5a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$YcmB3dkAdGRga3aFMRWuFuqwPFt6qEVHjuqHJpFoX.yjFD0J.VYC");
        }
    }
}
