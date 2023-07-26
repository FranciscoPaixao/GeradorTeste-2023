using GeradorTestes.Aplicacao.ModuloTeste;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;
using GeradorTestes.Infra.MassaDados;
using GeradorTestes.Infra.Orm;
using GeradorTestes.Infra.Pdf;
using GeradorTestes.Infra.Sql.ModuloDisciplina;
using GeradorTestes.Infra.Sql.ModuloMateria;
using GeradorTestes.Infra.Sql.ModuloQuestao;
using GeradorTestes.Infra.Sql.ModuloTeste;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace GeradorTestes.ConsoleApp
{

    internal class Program
    {
        static void Main(string[] args)
        {
            LimparTabelas();

            var disciplina = InserirDisciplina();

            InserirMateria();
        }

        private static void InserirMateria()
        {
            Console.Clear();

            var dbContext = new GeradorTestesDbContext();

            var disciplina = dbContext.Disciplinas.FirstOrDefault(x => x.Nome == "Matemática");

            var materia = new Materia("Adição de Unidades", SerieMateriaEnum.PrimeiraSerie, disciplina);

            dbContext.Materias.Add(materia);

            dbContext.SaveChanges();
        }

        private static Disciplina InserirDisciplina()
        {
            Console.Clear();

            var dbContext = new GeradorTestesDbContext();

            var disciplina = new Disciplina("Matemática");

            dbContext.Disciplinas.Add(disciplina);

            dbContext.SaveChanges();

            return disciplina;
        }

        private static void LimparTabelas()
        {
            Console.Clear();

            GeradorTestesDbContext dbContext = new GeradorTestesDbContext();

            dbContext.Disciplinas.RemoveRange(dbContext.Disciplinas);

            dbContext.Materias.RemoveRange(dbContext.Materias);

            dbContext.SaveChanges();
        }

        static void Main2(string[] args)
        {
            var configuracao = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var diretorioLogs = configuracao.GetSection("DiretorioLogs:Caminho").Value;

            IRepositorioDisciplina repositorioDisciplina = new RepositorioDisciplinaEmSql(connectionString);
            IRepositorioMateria repositorioMateria = new RepositorioMateriaEmSql(connectionString);
            IRepositorioQuestao repositorioQuestao = new RepositorioQuestaoEmSql(connectionString);
            IRepositorioTeste repositorioTeste = new RepositorioTesteEmSql(connectionString);

            IGeradorArquivo geradorRelatorio = new GeradorTesteEmPdf();
            ValidadorTeste validadorTeste = new ValidadorTeste();
            ServicoTeste servicoTeste = new ServicoTeste(repositorioTeste, repositorioQuestao, validadorTeste, geradorRelatorio);

            GeradorMassaDados geradorMassa = new GeradorMassaDados(repositorioDisciplina, repositorioMateria, repositorioQuestao, servicoTeste);

            geradorMassa.ConfigurarTesteMatematica();
        }
    }
}