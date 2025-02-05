using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "Services",
                type: "numeric(2)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Services",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "RegistryPhoneNumber",
                table: "Offices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId1",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId1",
                table: "Appointments",
                column: "ServiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments",
                column: "ServiceId1",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ServiceId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RegistryPhoneNumber",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "ServiceId1",
                table: "Appointments");

            migrationBuilder.AlterColumn<decimal>(
                name: "ServicePrice",
                table: "Services",
                type: "numeric(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Services",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
