using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorTestes.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class AddTBMateria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBMateria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Serie = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_TBMateria_DisciplinaId",
                table: "TBMateria",
                column: "DisciplinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBMateria");
        }
    }
}
