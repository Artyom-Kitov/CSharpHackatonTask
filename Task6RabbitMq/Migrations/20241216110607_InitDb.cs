using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task6RabbitMq.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hackaton",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    harmony = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hackaton", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "junior",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_junior", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teamlead",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teamlead", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "junior_preference",
                columns: table => new
                {
                    hackaton_id = table.Column<int>(type: "INTEGER", nullable: false),
                    junior_id = table.Column<int>(type: "INTEGER", nullable: false),
                    teamlead_id = table.Column<int>(type: "INTEGER", nullable: false),
                    priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_junior_preference", x => new { x.hackaton_id, x.junior_id, x.teamlead_id });
                    table.ForeignKey(
                        name: "FK_junior_preference_hackaton_hackaton_id",
                        column: x => x.hackaton_id,
                        principalTable: "hackaton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_junior_preference_junior_junior_id",
                        column: x => x.junior_id,
                        principalTable: "junior",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_junior_preference_teamlead_teamlead_id",
                        column: x => x.teamlead_id,
                        principalTable: "teamlead",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    hackaton_id = table.Column<int>(type: "INTEGER", nullable: false),
                    junior_id = table.Column<int>(type: "INTEGER", nullable: false),
                    teamlead_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => new { x.hackaton_id, x.junior_id, x.teamlead_id });
                    table.ForeignKey(
                        name: "FK_team_hackaton_hackaton_id",
                        column: x => x.hackaton_id,
                        principalTable: "hackaton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_team_junior_junior_id",
                        column: x => x.junior_id,
                        principalTable: "junior",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_team_teamlead_teamlead_id",
                        column: x => x.teamlead_id,
                        principalTable: "teamlead",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teamlead_preference",
                columns: table => new
                {
                    hackaton_id = table.Column<int>(type: "INTEGER", nullable: false),
                    teamlead_id = table.Column<int>(type: "INTEGER", nullable: false),
                    junior_id = table.Column<int>(type: "INTEGER", nullable: false),
                    priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teamlead_preference", x => new { x.hackaton_id, x.junior_id, x.teamlead_id });
                    table.ForeignKey(
                        name: "FK_teamlead_preference_hackaton_hackaton_id",
                        column: x => x.hackaton_id,
                        principalTable: "hackaton",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teamlead_preference_junior_junior_id",
                        column: x => x.junior_id,
                        principalTable: "junior",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teamlead_preference_teamlead_teamlead_id",
                        column: x => x.teamlead_id,
                        principalTable: "teamlead",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_junior_preference_junior_id",
                table: "junior_preference",
                column: "junior_id");

            migrationBuilder.CreateIndex(
                name: "IX_junior_preference_teamlead_id",
                table: "junior_preference",
                column: "teamlead_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_junior_id",
                table: "team",
                column: "junior_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_teamlead_id",
                table: "team",
                column: "teamlead_id");

            migrationBuilder.CreateIndex(
                name: "IX_teamlead_preference_junior_id",
                table: "teamlead_preference",
                column: "junior_id");

            migrationBuilder.CreateIndex(
                name: "IX_teamlead_preference_teamlead_id",
                table: "teamlead_preference",
                column: "teamlead_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "junior_preference");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "teamlead_preference");

            migrationBuilder.DropTable(
                name: "hackaton");

            migrationBuilder.DropTable(
                name: "junior");

            migrationBuilder.DropTable(
                name: "teamlead");
        }
    }
}
