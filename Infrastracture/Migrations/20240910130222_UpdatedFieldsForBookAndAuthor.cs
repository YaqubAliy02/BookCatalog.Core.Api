using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFieldsForBookAndAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "permission_name",
                table: "Permissions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlobPath",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thubnail",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorPhoto",
                table: "Authors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobPath",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Thubnail",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AuthorPhoto",
                table: "Authors");

            migrationBuilder.AlterColumn<string>(
                name: "permission_name",
                table: "Permissions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
