using GeradorTestes.Dominio.ModuloTeste;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloTeste
{
    public partial class TabelaTesteControl : UserControl
    {
        public TabelaTesteControl()
        {
            InitializeComponent();
            grid.ConfigurarGridZebrado();
            grid.ConfigurarGridSomenteLeitura();
            grid.Columns.AddRange(ObterColunas());
        }

        public DataGridViewColumn[] ObterColunas()
        {
            var colunas = new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Id", FillWeight=15F, Visible=false },

                new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Título", FillWeight=15F },

                new DataGridViewTextBoxColumn { Name = "Disciplina.Nome", HeaderText = "Disciplina", FillWeight=20F },

                new DataGridViewTextBoxColumn { Name = "Provao", HeaderText = "Tipo", FillWeight=25F },

                new DataGridViewTextBoxColumn { Name = "Materia.Nome", HeaderText = "Matéria", FillWeight=25F },


            };

            return colunas;
        }

        public Guid ObtemIdSelecionado()
        {
            return grid.SelecionarId();
        }

        public void AtualizarRegistros(List<Teste> testes)
        {
            grid.Rows.Clear();

            foreach (var teste in testes)
            {
                string disciplina = teste.Provao ? teste.Disciplina?.Nome : teste.Materia?.Disciplina?.Nome;

                grid.Rows.Add(teste.Id, teste.Titulo, disciplina,
                    teste.Provao ? "Provão" : "Fixação da Matéria", teste.Materia?.Nome);
            }
        }
    }
}
