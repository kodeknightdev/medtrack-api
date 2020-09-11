using System;
using MedTrack.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedTrack.Api.Migrations
{
    [DbContext(typeof(MedTrackContext))]
    [Migration("20260910122305_AddMedicationReminders")]
    public partial class AddMedicationReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ReminderTime",
                table: "Medications",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTakenAt",
                table: "Medications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTakenAt",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Medications");
        }
    }
}
