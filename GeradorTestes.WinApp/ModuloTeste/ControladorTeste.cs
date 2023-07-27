using GeradorTestes.Aplicacao.ModuloTeste;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloTeste;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloTeste
{
    public class ControladorTeste : ControladorBase
    {
        private IRepositorioDisciplina repositorioDisciplina;
        private IRepositorioTeste repositorioTeste;
        private ServicoTeste servicoTeste;

        private TabelaTesteControl tabelaTeste;

        public ControladorTeste(IRepositorioTeste repositorioTeste,
            IRepositorioDisciplina repositorioDisciplina,
            ServicoTeste servicoTeste)
        {
            this.repositorioDisciplina = repositorioDisciplina;
            this.repositorioTeste = repositorioTeste;
            this.servicoTeste = servicoTeste;
        }

        public override void Inserir()
        {
            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true, incluirQuestoes: true);

            TelaTesteForm tela = new TelaTesteForm(disciplinas);

            tela.onGravarRegistro += servicoTeste.Inserir;

            tela.ConfigurarTeste(new Teste());

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {               
                CarregarTestes();
            }
        }

        public override void Duplicar()
        {
            int id = tabelaTeste.ObtemIdSelecionado();

            Teste testeSelecionado = repositorioTeste.SelecionarPorId(id);

            if (testeSelecionado == null)
            {
                MessageBox.Show("Selecione um Teste primeiro",
                    "Duplicação de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true, incluirQuestoes: true);

            TelaTesteForm tela = new TelaTesteForm(disciplinas);

            tela.onGravarRegistro += servicoTeste.Inserir;

            testeSelecionado.RemoverQuestoes();

            tela.ConfigurarTeste(testeSelecionado);

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {               
                CarregarTestes();
            }
        }

        public override void Excluir()
        {
            int id = tabelaTeste.ObtemIdSelecionado();

            Teste testeSelecionado = repositorioTeste.SelecionarPorId(id, incluirQuestoes:true);

            if (testeSelecionado == null)
            {
                MessageBox.Show("Selecione um Teste primeiro",
                    "Exclusão de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult opcaoEscolhida = MessageBox.Show("Deseja realmente excluir o Teste?",
               "Exclusão de Testes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (opcaoEscolhida == DialogResult.OK)
            {
                Result resultado = servicoTeste.Excluir(testeSelecionado);

                if (resultado.IsFailed)
                {
                    MessageBox.Show(resultado.Errors[0].Message, "Exclusão de Testes", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                CarregarTestes();
            }
        }

        public override void Visualizar()
        {
            int id = tabelaTeste.ObtemIdSelecionado();

            Teste testeSelecionado = repositorioTeste.SelecionarPorId(id, incluirQuestoes:true);

            if (testeSelecionado == null)
            {
                MessageBox.Show("Selecione um Teste primeiro",
                    "Visualização de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            TelaVisualizacaoTesteForm tela = new TelaVisualizacaoTesteForm(testeSelecionado);
            tela.ShowDialog();
        }

        public override void GerarPdf()
        {
            int id = tabelaTeste.ObtemIdSelecionado();

            Teste testeSelecionado = repositorioTeste
                .SelecionarPorId(id, incluirQuestoes: true, incluirAlternativas: true, incluirMateria: true);

            if (testeSelecionado == null)
            {
                MessageBox.Show("Selecione um Teste primeiro",
                "Gerar Pdf de Testes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            TelaTestePdfForm tela = new TelaTestePdfForm(testeSelecionado);
            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                servicoTeste.GerarTesteEmPdf(testeSelecionado, tela.Diretorio, tela.GerarGabarito);
            }
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolboxTeste();
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaTeste == null)
                tabelaTeste = new TabelaTesteControl();

            CarregarTestes();

            return tabelaTeste;
        }

        private void CarregarTestes()
        {
            List<Teste> testes = repositorioTeste.SelecionarTodos(incluirMateria: true, incluirDisciplina: true);

            tabelaTeste.AtualizarRegistros(testes);

            mensagemRodape = string.Format("Visualizando {0} teste{1}", testes.Count, testes.Count == 1 ? "" : "s");

            TelaPrincipalForm.Instancia.AtualizarRodape(mensagemRodape);
        }
    }
}
