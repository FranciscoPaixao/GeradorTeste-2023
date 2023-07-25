using FluentValidation.TestHelper;

using GeradorTestes.Dominio.ModuloDisciplina;
using System.Runtime.ConstrainedExecution;

namespace GeradorTestes.TestesUnitarios.Dominio
{
    [TestClass]
    public class ValidadorDisciplinaTest
    {
        private Disciplina disciplina;
        private ValidadorDisciplina validador;

        public ValidadorDisciplinaTest()
        {
            disciplina = new Disciplina();

            validador = new ValidadorDisciplina();
        }

        [TestMethod]
        public void Nome_disciplina_nao_deve_ser_nulo_ou_vazio()
        {
            //action
            var resultado = validador.TestValidate(disciplina);

            //assert
            resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        }

        [TestMethod]
        public void Nome_disciplina_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            disciplina.Nome = "ab";

            //action
            var resultado = validador.TestValidate(disciplina);

            //assert
            resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        }

        [TestMethod]
        public void Nome_disciplina_deve_ser_composto_por_letras_e_numeros()
        {
            //arrange
            disciplina.Nome = "Artes @";

            //action
            var resultado = validador.TestValidate(disciplina);

            //assert
            resultado.ShouldHaveValidationErrorFor(x => x.Nome)
                .WithErrorMessage("'Nome' deve ser composto por letras e números.");
        }
    }
}
