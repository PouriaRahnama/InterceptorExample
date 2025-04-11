using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterceptorExample.web.Migrations
{
    /// <inheritdoc />
    public partial class InitalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    shortenUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    destenationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByBrowser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByIP = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");
        }
    }
}
