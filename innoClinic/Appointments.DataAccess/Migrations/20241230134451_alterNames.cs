using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class alterNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorFirstName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    DoctorSecondName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    DoctorSpecialization = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ServicePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    OfficeId = table.Column<string>(type: "text", nullable: false),
                    OfficeAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientFirstName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    PatientSecondName = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Complaints = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Conclusion = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Recomendations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DocumentUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_AppointmentId",
                table: "Results",
                column: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
