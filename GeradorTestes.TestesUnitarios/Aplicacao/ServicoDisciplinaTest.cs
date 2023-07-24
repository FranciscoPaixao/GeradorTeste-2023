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

        public ServicoDisciplinaTest()
        {
            repositorioDisciplinaMoq = new Mock<IRepositorioDisciplina>();
            validadorMoq = new Mock<IValidadorDisciplina>();
            servicoDisciplina = new ServicoDisciplina(repositorioDisciplinaMoq.Object, validadorMoq.Object);
        }

        [TestMethod]
        public void Quando_disciplina_eh_valida_deve_inserir() //cenário 1
        {      
            //arrange
            Disciplina disciplina = new Disciplina("Educação Física");          

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeSuccess();
            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Once());
        }

        [TestMethod]
        public void Quando_disciplina_duplicada_nao_deve_inserir() //cenário 4
        {              
            //arrange
            repositorioDisciplinaMoq.Setup(x => x.SelecionarPorNome("Educação Física"))
                .Returns(() => 
                { 
                    return new Disciplina(2, "Educação Física"); 
                });              

            validadorMoq.Setup(x => x.Validate(It.IsAny<Disciplina>()))                
                .Returns(() => 
                { 
                    return new ValidationResult(); 
                });

            Disciplina disciplina = new Disciplina("Educação Física");

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeFailure("Este nome 'Educação Física' já está sendo utilizado");

            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Never());
        }

        [TestMethod]
        public void Quando_disciplina_invalida_nao_deve_inserir() //cenário 2
        {           
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Disciplina>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Nome", "Nome não pode caracteres"));
                    return resultado;
                });

            Disciplina disciplina = new Disciplina("Educação Física");

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert             
            resultado.Should().BeFailure();
            repositorioDisciplinaMoq.Verify(x => x.Inserir(disciplina), Times.Never());
        }

        [TestMethod]
        public void Quando_ocorre_falha_ao_inserir_disciplina() //cenário 3
        {
            repositorioDisciplinaMoq.Setup(x => x.Inserir(It.IsAny<Disciplina>()))
                .Throws(() =>
                {
                    return new Exception();
                });

            Disciplina disciplina = new Disciplina("Educação Física");

            //action
            Result resultado = servicoDisciplina.Inserir(disciplina);

            //assert 
            resultado.Should().BeFailure("Falha ao tentar inserir disciplina.");
        }
    }
}
