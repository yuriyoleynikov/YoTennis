using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YoTennis.Migrations
{
    public partial class AddMatchInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchInfos",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(nullable: false),
                    FirstPlayer = table.Column<string>(nullable: true),
                    MatchScore = table.Column<string>(nullable: true),
                    MatchStartedAt = table.Column<DateTime>(nullable: false),
                    SecondPlayer = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Winner = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchInfos", x => x.MatchId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchInfos");
        }
    }
}
