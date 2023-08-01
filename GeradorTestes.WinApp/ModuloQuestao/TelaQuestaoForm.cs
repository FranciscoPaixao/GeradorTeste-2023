using GeradorTestes.WinApp.Compartilhado;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloQuestao
{
    public partial class TelaQuestaoForm : Form
    {
        private Questao questao;

        public event GravarRegistroDelegate<Questao> onGravarRegistro;

        public TelaQuestaoForm(List<Disciplina> disciplinas)
        {
            InitializeComponent();
            this.ConfigurarDialog();
            CarregarDisciplinas(disciplinas);
        }

        public Questao ObterQuestao()
        {
            questao.Id = Guid.Parse(txtId.Text);
            questao.Enunciado = txtEnunciado.Text;
            questao.Materia = (Materia)cmbMaterias.SelectedItem;
            questao.JaUtilizada = chkJaUtilizada.Checked;

            int i = 0;
            foreach (var item in listAlternativas.Items)
            {
                Alternativa a = (Alternativa)item;

                if (listAlternativas.GetItemChecked(i))
                    a.Correta = true;
                else
                    a.Correta = false;

                i++;
            }

            return questao;
        }

        public void ConfigurarQuestao(Questao questao)
        {
            this.questao = questao;

            txtId.Text = questao.Id.ToString();
            txtEnunciado.Text = questao.Enunciado;
            cmbDisciplinas.SelectedItem = questao.Materia?.Disciplina;
            cmbMaterias.SelectedItem = questao.Materia;
            chkJaUtilizada.Checked = questao.JaUtilizada;

            RecarregarAlternativas();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            this.questao = ObterQuestao();

            Result resultado = onGravarRegistro(questao);

            if (resultado.IsFailed)
            {
                string erro = resultado.Errors[0].Message;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            Alternativa alternativa = new Alternativa();

            alternativa.Letra = questao.GerarLetraAlternativa();
            alternativa.Resposta = txtResposta.Text;

            questao.AdicionarAlternativa(alternativa);

            RecarregarAlternativas();

            txtResposta.Focus();
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            var alternativa = listAlternativas.SelectedItem as Alternativa;

            if (alternativa != null)
            {
                questao.RemoverAlternativa(alternativa);

                listAlternativas.Items.Remove(alternativa);

                RecarregarAlternativas();
            }
        }

        private void RecarregarAlternativas()
        {
            listAlternativas.Items.Clear();

            int i = 0;
            foreach (var item in questao.Alternativas)
            {
                listAlternativas.Items.Add(item);

                if (item.Correta)
                    listAlternativas.SetItemChecked(i, true);

                i++;
            }
        }

        private void cmbDisciplinas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Disciplina disciplina = cmbDisciplinas.SelectedItem as Disciplina;

            if (disciplina != null)
                CarregarMaterias(disciplina.Materias);
        }

        private void CarregarMaterias(List<Materia> materias)
        {
            cmbMaterias.Items.Clear();

            foreach (var item in materias)
            {
                cmbMaterias.Items.Add(item);
            }
        }

        private void CarregarDisciplinas(List<Disciplina> disciplinas)
        {
            cmbDisciplinas.Items.Clear();

            foreach (var item in disciplinas)
            {
                cmbDisciplinas.Items.Add(item);
            }
        }

    }
}