using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantService.Migrations
{
    /// <inheritdoc />
    public partial class updatetenantEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tenant_subdomain",
                table: "tenant");

            migrationBuilder.RenameColumn(
                name: "subdomain",
                table: "tenant",
                newName: "orgnization");

            migrationBuilder.AddColumn<string>(
                name: "industry",
                table: "tenant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "logo",
                table: "tenant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "time_zone",
                table: "tenant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "tenant",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "industry",
                table: "tenant");

            migrationBuilder.DropColumn(
                name: "logo",
                table: "tenant");

            migrationBuilder.DropColumn(
                name: "time_zone",
                table: "tenant");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tenant");

            migrationBuilder.RenameColumn(
                name: "orgnization",
                table: "tenant",
                newName: "subdomain");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_subdomain",
                table: "tenant",
                column: "subdomain",
                unique: true);
        }
    }
}
