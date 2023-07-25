using FizzWare.NBuilder;
using FluentAssertions;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.TestesIntegracao.ModuloDisciplina
{
    [TestClass]
    public class RepositorioDisciplinaEmSqlTest : TestesIntegracaoBase
    {
        [TestMethod]
        public void Deve_inserir_disciplina()
        {            
            //arrange
            Disciplina disciplina = new Disciplina("abcdefeska");

            //action
            repositorioDisciplina.Inserir(disciplina);

            //assert
            repositorioDisciplina.SelecionarPorId(disciplina.Id).Should().Be(disciplina);
        }

        [TestMethod]
        public void Deve_editar_disciplina()
        {
            //arrange
            var disciplinaId = Builder<Disciplina>.CreateNew().Persist().Id;            

            var disciplina = repositorioDisciplina.SelecionarPorId(disciplinaId);
            disciplina.Nome = "História";

            //action
            repositorioDisciplina.Editar(disciplina);

            //assert
            repositorioDisciplina.SelecionarPorId(disciplina.Id)
                .Should().Be(disciplina);
        }

        [TestMethod]
        public void Deve_excluir_disciplina()
        {
            //arrange
            var disciplina = Builder<Disciplina>.CreateNew().Persist();

            //action
            repositorioDisciplina.Excluir(disciplina);

            //assert
            repositorioDisciplina.SelecionarPorId(disciplina.Id).Should().BeNull();
        }

        [TestMethod]
        public void Deve_selecionar_todas_disciplinas()
        {
            //arrange
            var matematica = Builder<Disciplina>.CreateNew().Persist();
            var portugues = Builder<Disciplina>.CreateNew().Persist();

            //action
            var disciplinas = repositorioDisciplina.SelecionarTodos();

            //assert
            disciplinas.Should().ContainInOrder(matematica, portugues);
            disciplinas.Should().HaveCount(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplinas_com_materias()
        {
            //arrange
            var matematica = Builder<Disciplina>.CreateNew().Persist();

            var adiciaoUnidades = Builder<Materia>.CreateNew().With(x => x.Disciplina = matematica).Persist();
            var adiciaoDezenas = Builder<Materia>.CreateNew().With(x => x.Disciplina = matematica).Persist();

            //action
            var disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true);

            //assert
            disciplinas[0].Materias.Should().ContainInOrder(adiciaoUnidades, adiciaoDezenas);
            disciplinas[0].Materias.Count.Should().Be(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplinas_com_materias_e_questoes()
        {
            //arrange
            var matematica = Builder<Disciplina>.CreateNew().Persist();

            var adiciaoUnidades = Builder<Materia>.CreateNew().With(x => x.Disciplina = matematica).Persist();

            var questao1 = Builder<Questao>.CreateNew().With(x => x.Materia = adiciaoUnidades).Persist();
            var questao2 = Builder<Questao>.CreateNew().With(x => x.Materia = adiciaoUnidades).Persist();            

            //action
            var disciplinasEncontradas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true, incluirQuestoes: true);

            //assert
            disciplinasEncontradas[0].Materias[0].Questoes.Should().ContainInOrder(questao1, questao2);
            disciplinasEncontradas[0].Materias[0].Questoes.Should().HaveCount(2);
        }

        [TestMethod]
        public void Deve_selecionar_disciplina_por_nome()
        {
            //arrange
            var matematica = Builder<Disciplina>.CreateNew().Persist();

            //action
            var disciplinasEncontrada = repositorioDisciplina.SelecionarPorNome(matematica.Nome);

            //assert
            disciplinasEncontrada.Should().Be(matematica);
        }

        [TestMethod]
        public void Deve_selecionar_disciplina_por_id()
        {
            //arrange
            var matematica = Builder<Disciplina>.CreateNew().Persist();

            //action
            var disciplinasEncontrada = repositorioDisciplina.SelecionarPorId(matematica.Id);

            //assert            
            disciplinasEncontrada.Should().Be(matematica);
        }


    }
}