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
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace GeradorTestes.WinApp.Compartilhado
{
    internal class IoC_ComInjecaoDependencia : IoC
    {
        private ServiceProvider container;

        public IoC_ComInjecaoDependencia()
        {
            var configuracao = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var servicos = new ServiceCollection();

            servicos.AddDbContext<IContextoPersistencia, GeradorTestesDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });

            servicos.AddTransient<ControladorDisciplina>();
            servicos.AddTransient<ServicoDisciplina>();
            servicos.AddTransient<IValidadorDisciplina, ValidadorDisciplina>();
            servicos.AddTransient<IRepositorioDisciplina, RepositorioDisciplinaEmOrm>();

            servicos.AddTransient<ControladorMateria>();
            servicos.AddTransient<ServicoMateria>();
            servicos.AddTransient<ValidadorMateria>();
            servicos.AddTransient<IRepositorioMateria, RepositorioMateriaEmOrm>();

            servicos.AddTransient<IRepositorioQuestao, RepositorioQuestaoEmOrm>();
            servicos.AddTransient<ValidadorQuestao>();
            servicos.AddTransient<ServicoQuestao>();
            servicos.AddTransient<ControladorQuestao>();

            servicos.AddTransient<IRepositorioTeste, RepositorioTesteEmOrm>();
            servicos.AddTransient<ValidadorTeste>();
            servicos.AddTransient<IGeradorArquivo, GeradorTesteEmPdf>();
            servicos.AddTransient<ServicoTeste>();
            servicos.AddTransient<ControladorTeste>();

            container = servicos.BuildServiceProvider();
        }

        public T Get<T>()
        {
            return container.GetService<T>();
        }
    }
}
