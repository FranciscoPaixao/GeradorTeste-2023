using GeradorTestes.Aplicacao.ModuloDisciplina;
using GeradorTestes.Aplicacao.ModuloMateria;
using GeradorTestes.Aplicacao.ModuloQuestao;
using GeradorTestes.Aplicacao.ModuloTeste;
using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;

using GeradorTestes.Infra.Orm.Compartilhado;
using GeradorTestes.Infra.Orm.ModuloDisciplina;
using GeradorTestes.Infra.Orm.ModuloMateria;
using GeradorTestes.Infra.Orm.ModuloQuestao;
using GeradorTestes.Infra.Orm.ModuloTeste;

using GeradorTestes.Infra.Pdf;
using GeradorTestes.WinApp.ModuloDisciplina;
using GeradorTestes.WinApp.ModuloMateria;
using GeradorTestes.WinApp.ModuloQuestao;
using GeradorTestes.WinApp.ModuloTeste;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GeradorTestes.WinApp
{
    public partial class TelaPrincipalForm : Form
    {
        private Dictionary<string, ControladorBase> controladores;

        private ControladorBase controlador;

        public TelaPrincipalForm()
        {
            InitializeComponent();

            Instancia = this;

            labelRodape.Text = string.Empty;
            labelTipoCadastro.Text = string.Empty;

            controladores = new Dictionary<string, ControladorBase>();

            ConfigurarControladores();
        }

        private void ConfigurarControladores()
        {
            var configuracao = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var optionsBuilder = new DbContextOptionsBuilder<GeradorTestesDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            GeradorTestesDbContext dbContext = new GeradorTestesDbContext(optionsBuilder.Options);

            //var migracoesPendentes = dbContext.Database.GetPendingMigrations();

            //if (migracoesPendentes.Count() > 0)
            //{
            //    dbContext.Database.Migrate();
            //}

            IRepositorioDisciplina repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);

            ValidadorDisciplina validadorDisciplina = new ValidadorDisciplina();

            ServicoDisciplina servicoDisciplina = new ServicoDisciplina(repositorioDisciplina, validadorDisciplina, dbContext);

            controladores.Add("ControladorDisciplina", new ControladorDisciplina(repositorioDisciplina, servicoDisciplina));

            IRepositorioMateria repositorioMateria = new RepositorioMateriaEmOrm(dbContext);

            ValidadorMateria validadorMateria = new ValidadorMateria();
            ServicoMateria servicoMateria = new ServicoMateria(repositorioMateria, validadorMateria, dbContext);

            controladores.Add("ControladorMateria", new ControladorMateria(repositorioMateria, repositorioDisciplina, servicoMateria));

            IRepositorioQuestao repositorioQuestao = new RepositorioQuestaoEmOrm(dbContext);

            ValidadorQuestao validadorQuestao = new ValidadorQuestao();
            ServicoQuestao servicoQuestao = new ServicoQuestao(repositorioQuestao, validadorQuestao);
            controladores.Add("ControladorQuestao", new ControladorQuestao(repositorioQuestao, repositorioDisciplina, servicoQuestao));

            IRepositorioTeste repositorioTeste = new RepositorioTesteEmOrm(dbContext);

            IGeradorArquivo geradorRelatorio = new GeradorTesteEmPdf();

            ValidadorTeste validadorTeste = new ValidadorTeste();
            ServicoTeste servicoTeste = new ServicoTeste(repositorioTeste, repositorioQuestao, validadorTeste, geradorRelatorio);

            controladores.Add("ControladorTeste", new ControladorTeste(repositorioTeste, repositorioDisciplina, servicoTeste));
        }

        public static TelaPrincipalForm Instancia
        {
            get;
            private set;
        }

        public void AtualizarRodape()
        {
            string mensagemRodape = controlador.ObterMensagemRodape();

            AtualizarRodape(mensagemRodape);
        }

        public void AtualizarRodape(string mensagem)
        {
            labelRodape.Text = mensagem;
        }

        private void disciplinasMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarTelaPrincipal(controladores["ControladorDisciplina"]);
        }

        private void materiasMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarTelaPrincipal(controladores["ControladorMateria"]);
        }

        private void questoesMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarTelaPrincipal(controladores["ControladorQuestao"]);
        }

        private void testesMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurarTelaPrincipal(controladores["ControladorTeste"]);
        }


        private void btnInserir_Click(object sender, EventArgs e)
        {
            controlador.Inserir();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            controlador.Editar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            controlador.Excluir();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            controlador.Filtrar();
        }

        private void btnGerarPdf_Click(object sender, EventArgs e)
        {
            controlador.GerarPdf();
        }

        private void btnDuplicar_Click(object sender, EventArgs e)
        {
            controlador.Duplicar();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            controlador.Visualizar();
        }

        private void ConfigurarBotoes(ConfiguracaoToolboxBase configuracao)
        {
            btnInserir.Enabled = configuracao.InserirHabilitado;
            btnEditar.Enabled = configuracao.EditarHabilitado;
            btnDuplicar.Enabled = configuracao.DuplicarHabilitado;
            btnExcluir.Enabled = configuracao.ExcluirHabilitado;
            btnFiltrar.Enabled = configuracao.FiltrarHabilitado;
            btnGerarPdf.Enabled = configuracao.GerarPdfHabilitado;
            btnVisualizar.Enabled = configuracao.VisualizarHabilitado;
        }

        private void ConfigurarTooltips(ConfiguracaoToolboxBase configuracao)
        {
            btnInserir.ToolTipText = configuracao.TooltipInserir;
            btnEditar.ToolTipText = configuracao.TooltipEditar;
            btnExcluir.ToolTipText = configuracao.TooltipExcluir;
            btnFiltrar.ToolTipText = configuracao.TooltipFiltrar;
            btnGerarPdf.ToolTipText = configuracao.TooltipGerarPdf;
            btnVisualizar.ToolTipText = configuracao.TooltipVisualizar;
        }

        private void ConfigurarTelaPrincipal(ControladorBase controlador)
        {
            this.controlador = controlador;

            ConfigurarToolbox();

            ConfigurarListagem();

            string mensagemRodape = controlador.ObterMensagemRodape();

            AtualizarRodape(mensagemRodape);
        }

        private void ConfigurarToolbox()
        {
            ConfiguracaoToolboxBase configuracao = controlador.ObtemConfiguracaoToolbox();

            if (configuracao != null)
            {
                toolbox.Enabled = true;

                labelTipoCadastro.Text = configuracao.TipoCadastro;

                ConfigurarTooltips(configuracao);

                ConfigurarBotoes(configuracao);
            }
        }

        private void ConfigurarListagem()
        {
            AtualizarRodape("");

            var listagemControl = controlador.ObtemListagem();

            panelRegistros.Controls.Clear();

            listagemControl.Dock = DockStyle.Fill;

            panelRegistros.Controls.Add(listagemControl);
        }


    }
}