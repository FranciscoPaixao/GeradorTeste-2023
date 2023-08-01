using GeradorTestes.WinApp.Compartilhado;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloTeste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloTeste
{
    public partial class TelaTesteForm : Form
    {
        private Teste teste;

        public GravarRegistroDelegate<Teste> onGravarRegistro;

        public TelaTesteForm(List<Disciplina> disciplinas)
        {
            InitializeComponent();
            this.ConfigurarDialog();
            CarregarDisciplinas(disciplinas);
        }
        public Teste ObterTeste()
        {
            teste.Id = Guid.Parse(txtId.Text);
            teste.Titulo = txtTitulo.Text;
            teste.Disciplina = cmbDisciplinas.SelectedItem as Disciplina;
            teste.DataGeracao = DateTime.Now;
            teste.Materia = cmbMaterias.SelectedItem as Materia;
            teste.Provao = chkProvao.Checked;
            teste.QuantidadeQuestoes = (int)txtQtdQuestoes.Value;

            return teste;
        }

        public void ConfigurarTeste(Teste teste)
        {
            this.teste = teste;

            txtId.Text = teste.Id.ToString();
            txtTitulo.Text = teste.Titulo;
            cmbDisciplinas.SelectedItem = teste.Provao ? teste.Disciplina : teste.Materia?.Disciplina;
            cmbMaterias.SelectedItem = teste.Materia;
            chkProvao.Checked = teste.Provao;
            txtQtdQuestoes.Value = teste.QuantidadeQuestoes;

            if (teste.Questoes != null)
            {
                foreach (var item in teste.Questoes)
                {
                    listQuestoes.Items.Add(item);
                }
            }
        }

        private void btnSortear_Click(object sender, EventArgs e)
        {            
            this.teste = ObterTeste();
            teste.QuestoesSorteadas = true;

            string[] erros = new ValidadorTeste().Validate(teste)
                .Errors.Select(x => x.ErrorMessage).ToArray();

            if (erros.Count() > 0)
            {
                string erro = erros[0];

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                return;
            }

            teste.SortearQuestoes();

            listQuestoes.Items.Clear();

            foreach (var item in teste.Questoes)
            {
                listQuestoes.Items.Add(item);
            }

            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            this.teste = ObterTeste();

            Result resultado = onGravarRegistro(teste);

            if (resultado.IsFailed)
            {
                string erro = resultado.Errors[0].Message;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void CarregarDisciplinas(List<Disciplina> disciplinas)
        {
            cmbDisciplinas.Items.Clear();

            foreach (Disciplina disciplina in disciplinas)
            {
                cmbDisciplinas.Items.Add(disciplina);
            }
        }

        private void CarregarMaterias(List<Materia> materias)
        {
            cmbMaterias.Items.Clear();

            foreach (Materia item in materias)
            {
                cmbMaterias.Items.Add(item);
            }
        }

        private void chkProvao_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProvao.Checked)
            {
                cmbMaterias.Enabled = false;
                cmbMaterias.Items.Clear();
            }
            else
            {
                cmbMaterias.Enabled = true;
                Disciplina disciplina = cmbDisciplinas.SelectedItem as Disciplina;

                if (disciplina != null)
                    CarregarMaterias(disciplina.Materias);
            }
        }

        private void cmbDisciplinas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Disciplina disciplina = cmbDisciplinas.SelectedItem as Disciplina;

            if (disciplina != null)
                CarregarMaterias(disciplina.Materias);
        }
    }
}
