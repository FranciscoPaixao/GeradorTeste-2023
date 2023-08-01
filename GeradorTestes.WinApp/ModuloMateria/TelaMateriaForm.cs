using GeradorTestes.WinApp.Compartilhado;
using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloMateria
{
    public partial class TelaMateriaForm : Form
    {
        private Materia materia;

        public event GravarRegistroDelegate<Materia> onGravarRegistro;

        public TelaMateriaForm(List<Disciplina> disciplinas)
        {
            InitializeComponent();
            this.ConfigurarDialog();
            CarregarDisciplinas(disciplinas);
            CarregarSeries();
        }

        public Materia ObterMateria()
        {
            materia.Id = Guid.Parse(txtId.Text);
            materia.Nome = txtNome.Text;
            materia.Serie = (SerieMateriaEnum)cmbSeries.SelectedValue;
            materia.Disciplina = (Disciplina)cmbDisciplinas.SelectedItem;

            return materia;
        }

        public void ConfigurarMateria(Materia materia)
        {
            this.materia = materia;

            txtId.Text = materia.Id.ToString();
            txtNome.Text = materia.Nome;
            cmbDisciplinas.SelectedItem = materia.Disciplina;
            cmbSeries.SelectedValue = materia.Serie;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            this.materia = ObterMateria();

            Result resultado = onGravarRegistro(materia);

            if (resultado.IsFailed)
            {
                string erro = resultado.Errors[0].Message;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void CarregarSeries()
        {
            SerieMateriaEnum[] series = Enum.GetValues<SerieMateriaEnum>();

            ArrayList items = new ArrayList();

            foreach (Enum serie in series)
            {
                var item = KeyValuePair.Create(serie, serie.GetDescription());
                items.Add(item);
            }

            cmbSeries.DataSource = items;
            cmbSeries.DisplayMember = "Value";
            cmbSeries.ValueMember = "Key";
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
