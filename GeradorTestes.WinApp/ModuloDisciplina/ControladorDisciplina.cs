using GeradorTestes.Aplicacao.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloDisciplina;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloDisciplina
{
    public class ControladorDisciplina : ControladorBase
    {
        private IRepositorioDisciplina repositorioDisciplina;

        private TabelaDisciplinaControl tabelaDisciplina;

        private ServicoDisciplina servicoDisciplina;

        public ControladorDisciplina(
            IRepositorioDisciplina repositorioDisciplina,
            ServicoDisciplina servicoDisciplina)
        {
            this.repositorioDisciplina = repositorioDisciplina;
            this.servicoDisciplina = servicoDisciplina;
        }

        public override void Inserir()
        {
            TelaDisciplinaForm tela = new TelaDisciplinaForm();

            tela.onGravarRegistro += servicoDisciplina.Inserir;

            tela.ConfigurarDisciplina(new Disciplina());

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarDisciplinas();
            }            
        }

        public override void Editar()
        {
            Guid id = tabelaDisciplina.ObtemIdSelecionado();

            Disciplina disciplinaSelecionada = repositorioDisciplina.SelecionarPorId(id);

            if (disciplinaSelecionada == null)
            {
                MessageBox.Show("Selecione uma disciplina primeiro",
                "Edição de Compromissos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaDisciplinaForm tela = new TelaDisciplinaForm();

            tela.onGravarRegistro += servicoDisciplina.Editar;

            tela.ConfigurarDisciplina(disciplinaSelecionada);

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarDisciplinas();                
            }
        }

        public override void Excluir()
        {
            Guid id = tabelaDisciplina.ObtemIdSelecionado();

            Disciplina disciplinaSelecionada = repositorioDisciplina.SelecionarPorId(id);

            if (disciplinaSelecionada == null)
            {
                MessageBox.Show("Selecione uma disciplina primeiro",
                "Exclusão de Disciplinas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult opcaoEscolhida = MessageBox.Show("Deseja realmente excluir a disciplina?",
               "Exclusão de Disciplinas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (opcaoEscolhida == DialogResult.OK)
            {
                Result resultado = servicoDisciplina.Excluir(disciplinaSelecionada);

                if (resultado.IsFailed)
                {
                    MessageBox.Show(resultado.Errors[0].Message, "Exclusão de Disciplinas", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                CarregarDisciplinas();
            }            
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolboxDisciplina();
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaDisciplina == null)
                tabelaDisciplina = new TabelaDisciplinaControl();

            CarregarDisciplinas();

            return tabelaDisciplina;
        }

        private void CarregarDisciplinas()
        {
            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos();

            tabelaDisciplina.AtualizarRegistros(disciplinas);

            mensagemRodape = string.Format("Visualizando {0} disciplina{1}", disciplinas.Count, disciplinas.Count == 1 ? "" : "s");

            TelaPrincipalForm.Instancia.AtualizarRodape(mensagemRodape);
        }
    }
}
