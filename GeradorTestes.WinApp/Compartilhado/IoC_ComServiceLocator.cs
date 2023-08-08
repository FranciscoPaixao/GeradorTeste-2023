using GeradorTestes.Aplicacao.ModuloDisciplina;
using GeradorTestes.Aplicacao.ModuloMateria;
using GeradorTestes.Aplicacao.ModuloQuestao;
using GeradorTestes.Aplicacao.ModuloTeste;
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

namespace GeradorTestes.WinApp.Compartilhado
{
    public class IoC_ComServiceLocator : IoC
    {
        private Dictionary<string, ControladorBase> controladores;

        public IoC_ComServiceLocator()
        {
            controladores = new Dictionary<string, ControladorBase>();

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

        public ControladorBase Get<T>()
        {
            Type tipo = typeof(T);

            string nome = tipo.Name;

            return controladores[nome];
        }
    }
}
