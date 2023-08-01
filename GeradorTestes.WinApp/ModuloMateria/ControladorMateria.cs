using GeradorTestes.Aplicacao.ModuloMateria;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeradorTestes.WinApp.ModuloMateria
{
    public class ControladorMateria : ControladorBase
    {
        private IRepositorioDisciplina repositorioDisciplina;
        private IRepositorioMateria repositorioMateria;

        private ServicoMateria servicoMateria;

        private TabelaMateriasControl tabelaMaterias;

        public ControladorMateria(
            IRepositorioMateria repositorioMateria,
            IRepositorioDisciplina repositorioDisciplina,
            ServicoMateria servicoMateria)
        {
            this.repositorioMateria = repositorioMateria;
            this.repositorioDisciplina = repositorioDisciplina;
            this.servicoMateria = servicoMateria;
        }

        public override void Inserir()
        {
            List<Disciplina> disciplinas = repositorioDisciplina.SelecionarTodos();

            TelaMateriaForm tela = new TelaMateriaForm(disciplinas);

            tela.onGravarRegistro += servicoMateria.Inserir;

            tela.ConfigurarMateria(new Materia());

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarMaterias();               
            }
        }

        public override void Editar()
        {
            Guid id = tabelaMaterias.ObtemIdSelecionado();

            Materia materiaSelecionada = repositorioMateria.SelecionarPorId(id);

            if (materiaSelecionada == null)
            {
                MessageBox.Show("Selecione uma matéria primeiro",
                "Edição de Matérias", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            List<Disciplina> materias = repositorioDisciplina.SelecionarTodos();

            TelaMateriaForm tela = new TelaMateriaForm(materias);

            tela.onGravarRegistro += servicoMateria.Editar;

            tela.ConfigurarMateria(materiaSelecionada);

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarMaterias();
            }
        }

        public override void Excluir()
        {
            Guid id = tabelaMaterias.ObtemIdSelecionado();

            Materia materiaSelecionada = repositorioMateria.SelecionarPorId(id);

            if (materiaSelecionada == null)
            {
                MessageBox.Show("Selecione uma matéria primeiro",
                "Exclusão de Matérias", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult opcaoEscolhida = MessageBox.Show("Deseja realmente excluir a matéria?",
               "Exclusão de Materias", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (opcaoEscolhida == DialogResult.OK)
            {
                Result resultado = servicoMateria.Excluir(materiaSelecionada);

                if (resultado.IsFailed)
                {
                    MessageBox.Show(resultado.Errors[0].Message, "Exclusão de Matérias", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                CarregarMaterias();
            }
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolboxMateria();
        }

        public override UserControl ObtemListagem()
        {
            if (tabelaMaterias == null)
                tabelaMaterias = new TabelaMateriasControl();

            CarregarMaterias();

            return tabelaMaterias;
        }        

        private void CarregarMaterias()
        {
            List<Materia> materias = repositorioMateria.SelecionarTodos(incluirDisciplina: true);

            tabelaMaterias.AtualizarRegistros(materias);

            mensagemRodape = string.Format("Visualizando {0} matéria{1}", materias.Count, materias.Count == 1 ? "" : "s");

            TelaPrincipalForm.Instancia.AtualizarRodape(mensagemRodape);
        }
    }
}
