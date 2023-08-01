using GeradorTestes.Aplicacao.ModuloMateria;
using GeradorTestes.Aplicacao.ModuloQuestao;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloQuestao;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloQuestao
{
    public class ControladorQuestao : ControladorBase
    {
        private IRepositorioDisciplina repositorioDisciplina;
        private IRepositorioQuestao repositorioQuestao;

        private ServicoQuestao servicoQuestao;

        private TabelaQuestaoControl tabelaQuestao;

        public ControladorQuestao(
            IRepositorioQuestao repositorioQuestao,
            IRepositorioDisciplina repositorioDisciplina,
            ServicoQuestao servicoQuestao)
        {
            this.repositorioQuestao = repositorioQuestao;
            this.repositorioDisciplina = repositorioDisciplina;
            this.servicoQuestao = servicoQuestao;
        }

        public override void Inserir()
        {
            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true);

            TelaQuestaoForm tela = new TelaQuestaoForm(disciplinas);

            tela.onGravarRegistro += servicoQuestao.Inserir;

            tela.ConfigurarQuestao(new Questao());

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarQuestoes();
            }
        }

        public override void Editar()
        {
            Guid id = tabelaQuestao.ObtemIdSelecionado();

            Questao questaoSelecionada = repositorioQuestao.SelecionarPorId(id, incluirAlternativas: true);

            if (questaoSelecionada == null)
            {
                MessageBox.Show("Selecione uma questão primeiro",
                "Edição de Questões", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos(incluirMaterias: true);

            TelaQuestaoForm tela = new TelaQuestaoForm(disciplinas);

            tela.onGravarRegistro += servicoQuestao.Editar;

            tela.ConfigurarQuestao(questaoSelecionada);

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarQuestoes();
            }
        }

        public override void Excluir()
        {
            var numero = tabelaQuestao.ObtemIdSelecionado();

            Questao questaoSelecionada = repositorioQuestao.SelecionarPorId(numero);

            if (questaoSelecionada == null)
            {
                MessageBox.Show("Selecione uma questão primeiro",
                "Edição de Questões", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult opcaoEscolhida = MessageBox.Show("Deseja realmente excluir a questão?",
               "Exclusão de Questões", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (opcaoEscolhida == DialogResult.OK)
            {
                Result resultado = servicoQuestao.Excluir(questaoSelecionada);

                if (resultado.IsFailed)
                {
                    MessageBox.Show(resultado.Errors[0].Message, "Exclusão de Questões", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                CarregarQuestoes();
            }            
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolboxQuestao();
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaQuestao == null)
                tabelaQuestao = new TabelaQuestaoControl();

            CarregarQuestoes();

            return tabelaQuestao;
        }

        private void CarregarQuestoes()
        {
            List<Questao> questoes = repositorioQuestao.SelecionarTodos();

            tabelaQuestao.AtualizarRegistros(questoes);

            mensagemRodape = string.Format("Visualizando {0} quest{1}", questoes.Count, questoes.Count == 1 ? "ão" : "ões");

            TelaPrincipalForm.Instancia.AtualizarRodape(mensagemRodape);
        }
    }
}