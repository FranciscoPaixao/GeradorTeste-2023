using GeradorTestes.Dominio.ModuloTeste;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace GeradorTestes.TestesUnitarios.Dominio
{

    [TestClass]
    public class PeriodoEmTextoTest
    {
        //classes de equivalência
        /**
         * Caso o Teste tenha mais de 24 horas uma mensagem deve aparecer:
            -Teste realizado 1 dia atrás
         */

        [TestMethod]
        [DataRow(1, "Teste realizado 1 dia atrás")]
        [DataRow(2, "Teste realizado 2 dias atrás")]
        [DataRow(3, "Teste realizado 3 dias atrás")]
        [DataRow(4, "Teste realizado 4 dias atrás")]
        [DataRow(29, "Teste realizado 29 dias atrás")]
        public void Deve_retornar_teste_em_dias_atras(int qtdDias, string resultadoEsperado)
        {
            //arrange
            PeriodoEmTexto periodo = new PeriodoEmTexto(DateTime.Now, DateTime.Now.AddDays(-qtdDias));

            //action
            string resultadoAtual = periodo.ObterPeriodo();

            //assert
            Assert.AreEqual(resultadoEsperado, resultadoAtual);
        }
       
        [TestMethod]
        [DataRow(1, "Teste realizado 1 mês atrás")]
        [DataRow(2, "Teste realizado 2 meses atrás")]
        [DataRow(3, "Teste realizado 3 meses atrás")]
        public void Deve_retornar_teste_realizado_meses_atras(int qtdMeses, string resultadoEsperado)
        {
            //arrange
            PeriodoEmTexto periodo = new PeriodoEmTexto(DateTime.Now, DateTime.Now.AddMonths(-qtdMeses));

            //action
            string resultadoAtual = periodo.ObterPeriodo();

            //assert
            Assert.AreEqual(resultadoEsperado, resultadoAtual);
        }

        [TestMethod]
        [DataRow(1, "Teste realizado 1 ano atrás")]
        [DataRow(2, "Teste realizado 2 anos atrás")]
        [DataRow(3, "Teste realizado 3 anos atrás")]
        public void Deve_retornar_teste_realizado_anos_atras(int qtdAnos, string resultadoEsperado)
        {
            //arrange
            PeriodoEmTexto periodo = new PeriodoEmTexto(DateTime.Now, DateTime.Now.AddYears(-qtdAnos));

            //action
            string resultadoAtual = periodo.ObterPeriodo();

            //assert
            Assert.AreEqual(resultadoEsperado, resultadoAtual);
        }


    }
}
