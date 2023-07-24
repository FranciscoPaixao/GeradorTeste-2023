using FluentAssertions;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeradorTestes.TestesUnitarios
{
    [TestClass]
    public class DisciplinaTest
    {
        Disciplina matematica;
        Materia adiciaoUnidades;
        private Materia adiciaoDezenas;

        public DisciplinaTest()
        {
            matematica = new Disciplina("Matemática");

            adiciaoUnidades = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, matematica);
        }

        [TestMethod]
        public void Materias_Devem_ser_diferentes_de_null()
        {
            matematica.Materias.Should().NotBeNull();
        }

        [TestMethod]
        public void Deve_permitir_adicionar_materias_na_disciplina()
        {            
            //Cenário -- Arrange
            Materia adiciaoDezenas = new Materia("Adição de Dezenas", SerieMateriaEnum.PrimeiraSerie, matematica);

            //Ação -- Action
            matematica.AdicionarMateria(adiciaoUnidades);
            matematica.AdicionarMateria(adiciaoDezenas);

            //Verificação -- Assert            
            matematica.Materias.Count.Should().Be(2);
        }

        [TestMethod]
        public void Nao_deve_adicionar_materias_iguais_na_disciplina()
        {
            //arrange
            adiciaoUnidades = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, matematica);
            
            //action
            matematica.AdicionarMateria(adiciaoUnidades);
            matematica.AdicionarMateria(adiciaoUnidades);

            //assert
            matematica.Materias.Should().HaveCount(1);
        }

        [TestMethod]
        public void Deve_obter_todas_questoes_das_materias()
        {
            //arrange
            adiciaoUnidades = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, matematica);
            Questao q1 = new Questao("Quanto é 2 + 2 ?", adiciaoUnidades);
            Questao q2 = new Questao("Quanto é 3 + 3 ?", adiciaoUnidades);

            adiciaoUnidades.AdicionaQuestao(q1);
            adiciaoUnidades.AdicionaQuestao(q2);

            adiciaoDezenas = new Materia("Adição de Dezenas", SerieMateriaEnum.PrimeiraSerie, matematica);

            Questao q3 = new Questao("Quanto é 20 + 20 ?", adiciaoDezenas);
            Questao q4 = new Questao("Quanto é 30 + 30 ?", adiciaoDezenas);

            adiciaoDezenas.AdicionaQuestao(q3);
            adiciaoDezenas.AdicionaQuestao(q4);
            
            matematica.AdicionarMateria(adiciaoUnidades);
            matematica.AdicionarMateria(adiciaoDezenas);

            //action
            List<Questao> questoes = matematica.ObterTodasQuestoes();

            //Assert
            questoes.Should().HaveCount(4);
        }
    }
}