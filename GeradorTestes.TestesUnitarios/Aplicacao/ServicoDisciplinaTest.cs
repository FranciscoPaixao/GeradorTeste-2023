using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation.Results;
using GeradorTestes.Aplicacao.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloDisciplina;
using Moq;

namespace GeradorTestes.TestesUnitarios.Aplicacao
{
    [TestClass]
    public class ServicoDisciplinaTest
    {
        Mock<IRepositorioDisciplina> repositorioDisciplinaMoq;
        Mock<IValidadorDisciplina> validadorMoq;

        private ServicoDisciplina servicoDisciplina;

        Disciplina disciplina;

        public ServicoDisciplinaTest()
        {
            repositorioDisciplinaMoq = new Mock<IRepositorioDisciplina>();
            validadorMoq = new Mock<IValidadorDisciplina>();
            servicoDisciplina = new ServicoDisciplina(repositorioDisciplinaMoq.Object, validadorMoq.Object);
            disciplina = new Disciplina("Educação Física");
        }

        [TestMethod]
        public void Deve_inserir_disciplina_caso_ela_for_valida() //cenário 1
        {
            //arrange
            disciplina = new Disciplina("Educação Física");

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeSuccess();
            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Once());
        }

        [TestMethod]
        public void Nao_deve_inserir_disciplina_caso_ela_seja_invalida() //cenário 2
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Disciplina>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Nome", "Nome não pode ter caracteres especiais"));
                    return resultado;
                });

            //action
            var resultado = servicoDisciplina.Inserir(disciplina);

            //assert             
            resultado.Should().BeFailure();
            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Never());
        }

        [TestMethod]
        public void Nao_deve_inserir_disciplina_caso_o_nome_ja_esteja_cadastrado() //cenário 3
        {
            //arrange
            string nomeDisciplina = "Educação Física";
            repositorioDisciplinaMoq.Setup(x => x.SelecionarPorNome(nomeDisciplina))
                .Returns(() =>
                {
                    return new Disciplina(2, nomeDisciplina);
                });
          
            //action
            var resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be($"Este nome '{nomeDisciplina}' já está sendo utilizado");
            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Never());
        }

        [TestMethod]
        public void Deve_tratar_erro_caso_ocorra_falha_ao_tentar_inserir_disciplina() //cenário 4
        {
            repositorioDisciplinaMoq.Setup(x => x.Inserir(It.IsAny<Disciplina>()))
                .Throws(() =>
                {
                    return new Exception();
                });

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be("Falha ao tentar inserir disciplina.");
        }


        [TestMethod]
        public void Deve_editar_disciplina_caso_ela_for_valida() //cenário 1
        {
            //arrange           
            disciplina = new Disciplina(1, "Artes");

            //action
            Result resultado = servicoDisciplina.Editar(disciplina);

            //assert 
            resultado.Should().BeSuccess();
            repositorioDisciplinaMoq.Verify(x => x.Editar(disciplina), Times.Once());
        }

        [TestMethod]
        public void Nao_deve_editar_disciplina_caso_ela_seja_invalida() //cenário 2
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Disciplina>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Nome", "Nome não pode ter caracteres especiais"));
                    return resultado;
                });

            //action
            var resultado = servicoDisciplina.Editar(disciplina);

            //assert             
            resultado.Should().BeFailure();
            repositorioDisciplinaMoq.Verify(x => x.Editar(disciplina), Times.Never());
        }

        [TestMethod]
        public void Deve_editar_disciplina_com_o_mesmo_nome() //cenário 3
        {
            //arrange
            repositorioDisciplinaMoq.Setup(x => x.SelecionarPorNome("Educação Física"))
                 .Returns(() =>
                 {
                     return new Disciplina(1, "Educação Física");
                 });

            Disciplina outraDisciplina = new Disciplina(1, "Educação Física");

            //action
            var resultado = servicoDisciplina.Editar(outraDisciplina);

            //assert 
            resultado.Should().BeSuccess();

            repositorioDisciplinaMoq.Verify(x => x.Editar(outraDisciplina), Times.Once());
        }

        [TestMethod]
        public void Nao_deve_editar_disciplina_caso_o_nome_ja_esteja_cadastrado() //cenário 4
        {
            //arrange
            repositorioDisciplinaMoq.Setup(x => x.SelecionarPorNome("Educação Física"))
                 .Returns(() =>
                 {
                     return new Disciplina(1, "Educação Física");
                 });

            Disciplina novaDisciplina = new Disciplina("Educação Física");

            //action
            var resultado = servicoDisciplina.Editar(novaDisciplina);

            //assert 
            resultado.Should().BeFailure();

            repositorioDisciplinaMoq.Verify(x => x.Editar(novaDisciplina), Times.Never());
        }

        [TestMethod]
        public void Deve_tratar_erro_caso_ocorra_falha_ao_tentar_editar_disciplina() //cenário 5
        {
            repositorioDisciplinaMoq.Setup(x => x.Editar(It.IsAny<Disciplina>()))
                .Throws(() =>
                {
                    return new Exception();
                });

            //action
            Result resultado = servicoDisciplina.Editar(disciplina);

            //assert 
            resultado.Should().BeFailure();
            resultado.Errors[0].Message.Should().Be("Falha ao tentar editar disciplina.");
        }


        [TestMethod]
        public void Deve_excluir_disciplina_caso_ela_esteja_cadastrada() //cenário 1
        {
            //arrange
            var disciplina = new Disciplina(1, "Matemática");

            repositorioDisciplinaMoq.Setup(x => x.Existe(disciplina))
               .Returns(() =>
               {
                   return true;
               });

            //action
            var resultado = servicoDisciplina.Excluir(disciplina);

            //assert 
            resultado.Should().BeSuccess();
            repositorioDisciplinaMoq.Verify(x => x.Excluir(disciplina), Times.Once());
        }

        [TestMethod]
        public void Nao_deve_excluir_disciplina_caso_ela_nao_esteja_cadastrada() //cenário 2
        {
            //arrange

            var disciplina = new Disciplina(1, "Matemática");

            repositorioDisciplinaMoq.Setup(x => x.Existe(disciplina))
               .Returns(() =>
               {
                   return false;
               });

            //action
            var resultado = servicoDisciplina.Excluir(disciplina);

            //assert 
            resultado.Should().BeFailure();
            repositorioDisciplinaMoq.Verify(x => x.Excluir(disciplina), Times.Never());
        }

        [TestMethod]
        public void Nao_deve_excluir_disciplina_caso_ela_esteja_relacionada_com_materia() //cenário 3
        {            
            var disciplina = new Disciplina(1, "Matemática");

            repositorioDisciplinaMoq.Setup(x => x.Existe(disciplina))
               .Returns(() =>
               {
                   return true;
               });

            // como configurar um método para ele lançar uma sqlexception utilizando moq

            repositorioDisciplinaMoq.Setup(x => x.Excluir(It.IsAny<Disciplina>()))
                .Throws(() =>
                {
                    /** Solução utilizando Chat GPT
                       *  var exception = (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
                       *  
                       *  FieldInfo messageField = typeof(SqlException).GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
                       *  if (messageField != null)
                       *  {
                       *      messageField.SetValue(exception, "Esta é uma mensagem personalizada para a exceção SQL.");
                       *  }
                       */

                    return SqlExceptionCreator.NewSqlException(errorMessage: "FK_TBMateria_TBDisciplina");
                });

            //action
            Result resultado = servicoDisciplina.Excluir(disciplina);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be("Esta disciplina está relacionada com uma matéria e não pode ser excluída");
        }

        [TestMethod]
        public void Deve_tratar_erro_caso_ocorra_falha_ao_tentar_excluir_disciplina() //cenário 4
        {
            var disciplina = new Disciplina(1, "Matemática");

            repositorioDisciplinaMoq.Setup(x => x.Existe(disciplina))
              .Throws(() =>
              {
                  return SqlExceptionCreator.NewSqlException();
              });

            //action
            Result resultado = servicoDisciplina.Excluir(disciplina);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be("Falha ao tentar excluir disciplina");
        }

    }    
}