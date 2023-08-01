using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorTestes.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBDisciplina",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBDisciplina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBMateria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Serie = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBMateria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBMateria_TBDisciplina",
                        column: x => x.DisciplinaId,
                        principalTable: "TBDisciplina",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBQuestao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Enunciado = table.Column<string>(type: "varchar(500)", nullable: false),
                    MateriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JaUtilizada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBQuestao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBQuestao_TBMateria",
                        column: x => x.MateriaId,
                        principalTable: "TBMateria",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBTeste",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(250)", nullable: false),
                    Provao = table.Column<bool>(type: "bit", nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisciplinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MateriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuantidadeQuestoes = table.Column<int>(type: "int", nullable: false),
                    QuestoesSorteadas = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBTeste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBTeste_TBDisciplina",
                        column: x => x.DisciplinaId,
                        principalTable: "TBDisciplina",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBTeste_TBMateria",
                        column: x => x.MateriaId,
                        principalTable: "TBMateria",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TBAlternativa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Correta = table.Column<bool>(type: "bit", nullable: false),
                    Letra = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    QuestaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TBTeste_TBQuestao",
                columns: table => new
                {
                    QuestoesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TesteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBTeste_TBQuestao", x => new { x.QuestoesId, x.TesteId });
                    table.ForeignKey(
                        name: "FK_TBTeste_TBQuestao_TBQuestao_QuestoesId",
                        column: x => x.QuestoesId,
                        principalTable: "TBQuestao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBTeste_TBQuestao_TBTeste_TesteId",
                        column: x => x.TesteId,
                        principalTable: "TBTeste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBAlternativa_QuestaoId",
                table: "TBAlternativa",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TBMateria_DisciplinaId",
                table: "TBMateria",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_TBQuestao_MateriaId",
                table: "TBQuestao",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TBTeste_DisciplinaId",
                table: "TBTeste",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_TBTeste_MateriaId",
                table: "TBTeste",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_TBTeste_TBQuestao_TesteId",
                table: "TBTeste_TBQuestao",
                column: "TesteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBAlternativa");

            migrationBuilder.DropTable(
                name: "TBTeste_TBQuestao");

            migrationBuilder.DropTable(
                name: "TBQuestao");

            migrationBuilder.DropTable(
                name: "TBTeste");

            migrationBuilder.DropTable(
                name: "TBMateria");

            migrationBuilder.DropTable(
                name: "TBDisciplina");
        }
    }
}
