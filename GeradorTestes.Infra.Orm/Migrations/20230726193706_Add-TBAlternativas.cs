using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorTestes.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class AddTBAlternativas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBAlternativa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Correta = table.Column<bool>(type: "bit", nullable: false),
                    Letra = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    QuestaoId = table.Column<int>(type: "int", nullable: false),
                    Resposta = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAlternativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBAlternativa_TBQuestao",
                        column: x => x.QuestaoId,
                        principalTable: "TBQuestao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBAlternativa_QuestaoId",
                table: "TBAlternativa",
                column: "QuestaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBAlternativa");
        }
    }
}
