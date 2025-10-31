using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.SyncService.WebDashboard.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmEntities1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Services",
                newName: "ServiceName");

            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "Services",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Contracts",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Contracts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ContractName",
                table: "Contracts",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Services",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ContractCode",
                table: "Contracts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Contracts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContactId",
                table: "Contracts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ServiceId",
                table: "Contracts",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Contacts_ContactId",
                table: "Contracts",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Services_ServiceId",
                table: "Contracts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Contacts_ContactId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Services_ServiceId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContactId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ServiceId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ContractCode",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "Services",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Services",
                newName: "ExpireDate");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Contracts",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Contracts",
                newName: "ContractName");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Contracts",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "Services",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Services",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contracts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
