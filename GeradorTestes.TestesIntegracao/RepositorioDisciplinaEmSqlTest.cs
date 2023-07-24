using FluentAssertions;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Infra.Sql.ModuloDisciplina;
using GeradorTestes.Infra.Sql.ModuloMateria;
using GeradorTestes.Infra.Sql.ModuloQuestao;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeradorTestes.TestesIntegracao
{
    [TestClass]
    public class RepositorioDisciplinaEmSqlTest
    {        
        private IRepositorioDisciplina repositorioDisciplina;
        private IRepositorioMateria repositorioMateria;
        private IRepositorioQuestao repositorioQuestao;
        public RepositorioDisciplinaEmSqlTest()
        {
            var configuracao = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            repositorioDisciplina = new RepositorioDisciplinaEmSql(connectionString);
            repositorioMateria = new RepositorioMateriaEmSql(connectionString);
            repositorioQuestao = new RepositorioQuestaoEmSql(connectionString);

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlLimpezaTabela =
                @"
                DELETE FROM [DBO].[TBQUESTAO]
                DBCC CHECKIDENT ('[TBQUESTAO]', RESEED, 0);

                DELETE FROM [DBO].[TBMATERIA]
                DBCC CHECKIDENT ('[TBMATERIA]', RESEED, 0);

                DELETE FROM [DBO].[TBDISCIPLINA]
                DBCC CHECKIDENT ('[TBDISCIPLINA]', RESEED, 0);";

            SqlCommand comando = new SqlCommand(sqlLimpezaTabela, sqlConnection);

            sqlConnection.Open();

            comando.ExecuteNonQuery();

            sqlConnection.Close();
        }

        [TestMethod]
        public void Deve_inserir_disciplina()
        {
            //arrange
            Disciplina disciplina = new Disciplina("Matemática");
           
            //action
            repositorioDisciplina.Inserir(disciplina);

            //assert

            Disciplina disciplinaEncontrada = repositorioDisciplina.SelecionarPorId(disciplina.Id);

            disciplinaEncontrada.Should().Be(disciplina);
        }

        [TestMethod]
        public void Deve_editar_disciplina()
        {
            //arrange
            Disciplina disciplina = new Disciplina("Matemática");

            repositorioDisciplina.Inserir(disciplina);

            Disciplina disciplinaAtualizada = repositorioDisciplina.SelecionarPorId(disciplina.Id);

            disciplinaAtualizada.Nome = "História";

            //action
            repositorioDisciplina.Editar(disciplinaAtualizada);

            //assert

            Disciplina disciplinaEncontrada = repositorioDisciplina.SelecionarPorId(disciplinaAtualizada.Id);

            disciplinaEncontrada.Should().Be(disciplinaAtualizada);
        }

        [TestMethod]
        public void Deve_excluir_disciplina()
        {
            //arrange
            Disciplina disciplina = new Disciplina("Matemática");

            repositorioDisciplina.Inserir(disciplina);

            //action
            repositorioDisciplina.Excluir(disciplina);

            //assert

            Disciplina disciplinaEncontrada = repositorioDisciplina.SelecionarPorId(disciplina.Id);

            disciplinaEncontrada.Should().BeNull();            
        }

        [TestMethod]
        public void Deve_selecionar_todas_disciplinas()
        {
            //arrange
            var matematica = new Disciplina("Matemática");
            var portugues = new Disciplina("Português");

            repositorioDisciplina.Inserir(matematica);
            repositorioDisciplina.Inserir(portugues);

            //action
            var disciplinas = repositorioDisciplina.SelecionarTodos();

            //assert
            disciplinas[0].Should().Be(matematica);
            disciplinas[1].Should().Be(portugues);

            disciplinas.Should().HaveCount(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplinas_com_materias()
        {
            //arrange
            var matematica = new Disciplina("Matemática");           

            repositorioDisciplina.Inserir(matematica);

            var adiciaoUnidades = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, matematica);
            var adiciaoDezenas = new Materia("Adição de Dezenas", SerieMateriaEnum.PrimeiraSerie, matematica);

            repositorioMateria.Inserir(adiciaoUnidades);
            repositorioMateria.Inserir(adiciaoDezenas);

            //action
            var disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true);

            //assert
            disciplinas[0].Materias[0].Should().Be(adiciaoUnidades);
            disciplinas[0].Materias[1].Should().Be(adiciaoDezenas);

            disciplinas[0].Materias.Count.Should().Be(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplinas_com_materias_e_questoes()
        {
            //arrange
            var matematica = new Disciplina("Matemática");

            repositorioDisciplina.Inserir(matematica);

            var adiciaoUnidades = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, matematica);

            var questao1 = new Questao("Quanto é 2 + 2 ?", adiciaoUnidades);
            var questao2 = new Questao("Quanto é 3 + 3 ?", adiciaoUnidades);

            repositorioMateria.Inserir(adiciaoUnidades);

            repositorioQuestao.Inserir(questao1);
            repositorioQuestao.Inserir(questao2);

            //action
            var disciplinasEncontradas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true, incluirQuestoes: true);

            //assert
            disciplinasEncontradas[0].Materias[0].Questoes[0].Should().Be(questao1);
            disciplinasEncontradas[0].Materias[0].Questoes[1].Should().Be(questao2);

            disciplinasEncontradas[0].Materias[0].Questoes.Should().HaveCount(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplina_por_nome()
        {
            //arrange
            var matematica = new Disciplina("Matemática");

            repositorioDisciplina.Inserir(matematica);

            //action
            var disciplinasEncontrada = repositorioDisciplina.SelecionarPorNome(matematica.Nome);

            //assert
            disciplinasEncontrada.Should().Be(matematica);
        }

        [TestMethod]
        public void Deve_selecionar_disciplina_por_id()
        {
            //arrange
            var matematica = new Disciplina("Matemática");

            repositorioDisciplina.Inserir(matematica);

            //action
            var disciplinasEncontrada = repositorioDisciplina.SelecionarPorId(matematica.Id);

            //assert            
            disciplinasEncontrada.Should().Be(matematica);
        }


    }
}