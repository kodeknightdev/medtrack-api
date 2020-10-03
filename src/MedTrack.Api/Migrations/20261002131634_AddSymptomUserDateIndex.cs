using System;
using MedTrack.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedTrack.Api.Migrations
{
    [DbContext(typeof(MedTrackContext))]
    [Migration("20261002131634_AddSymptomUserDateIndex")]
    public partial class AddSymptomUserDateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SymptomEntries_UserId",
                table: "SymptomEntries");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomEntries_UserId_OccurredAt",
                table: "SymptomEntries",
                columns: new[] { "UserId", "OccurredAt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SymptomEntries_UserId_OccurredAt",
                table: "SymptomEntries");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomEntries_UserId",
                table: "SymptomEntries",
                column: "UserId");
        }
    }
}
