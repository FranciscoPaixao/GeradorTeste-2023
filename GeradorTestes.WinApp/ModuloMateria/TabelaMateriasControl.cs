using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloMateria;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloMateria
{
    public partial class TabelaMateriasControl : UserControl
    {
        public TabelaMateriasControl()
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
                new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Id", FillWeight=15F },

                new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", FillWeight=35F },

                new DataGridViewTextBoxColumn { Name = "Disciplina.Nome", HeaderText = "Disciplina", FillWeight=25F },

                new DataGridViewTextBoxColumn { Name = "Serie", HeaderText = "Série", FillWeight=25F },
            };

            return colunas;
        }

        public int ObtemIdSelecionado()
        {
            return grid.SelecionarId();
        }

        public void AtualizarRegistros(List<Materia> materias)
        {
            grid.Rows.Clear();

            foreach (var materia in materias)
            {                
                grid.Rows.Add(materia.Id, materia.Nome, materia.Disciplina?.Nome, materia.Serie.GetDescription());
            }
        }
    }
}
