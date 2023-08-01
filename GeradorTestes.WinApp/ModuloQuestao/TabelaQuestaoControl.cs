using GeradorTestes.Dominio.ModuloQuestao;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloQuestao
{
    public partial class TabelaQuestaoControl : UserControl
    {
        public TabelaQuestaoControl()
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
                new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Id", FillWeight=15F},

                new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", FillWeight=35F },

                new DataGridViewTextBoxColumn { Name = "JaUtilizada", HeaderText = "Já foi utilizada?", FillWeight=20F },

                new DataGridViewTextBoxColumn { Name = "Materia.Nome", HeaderText = "Matéria", FillWeight=15F },

                new DataGridViewTextBoxColumn { Name = "Disciplina.Nome", HeaderText = "Disciplina", FillWeight=15F }

            };

            return colunas;
        }

        public void AtualizarRegistros(List<Questao> questoes)
        {
            grid.Rows.Clear();

            foreach (var questao in questoes)
            {
                string stituacao = questao.JaUtilizada ? "Indisponível :-|" : "Disponível para uso :-)";

                grid.Rows.Add(questao.Id, questao.Enunciado, stituacao, questao?.Materia?.Nome, questao.Materia?.Disciplina?.Nome);
            }
        }

        public Guid ObtemIdSelecionado()
        {
            return grid.SelecionarId();
        }
    }
}
