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
using Microsoft.EntityFrameworkCore;
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

            InserirDisciplina();

            InserirMateria();

            InserirQuestoes();

            InserirTeste();
        }

        private static void InserirTeste()
        {
            Console.Clear();

            var dbContext = new GeradorTestesDbContext();

            var disciplina = dbContext.Disciplinas.FirstOrDefault(x => x.Nome == "Matemática");

            var materia = dbContext.Materias
                .Include(x => x.Questoes)
                    .ThenInclude(x => x.Alternativas)
                .FirstOrDefault(x => x.Nome.Contains("Adição"));

            Teste novoTeste = new Teste();

            novoTeste.Titulo = "Revisão sobre Adição de Unidades";
            novoTeste.Disciplina = disciplina;
            novoTeste.Materia = materia;
            novoTeste.Provao = false;
            novoTeste.QuantidadeQuestoes = 5;
            novoTeste.SortearQuestoes();
            novoTeste.DataGeracao = DateTime.Now;

            dbContext.Testes.Add(novoTeste);

            dbContext.SaveChanges();
        }

        private static void InserirQuestoes()
        {
            Console.Clear();

            var dbContext = new GeradorTestesDbContext();

            var materia = dbContext.Materias.FirstOrDefault(x => x.Nome.Contains("Adição"));

            for (int numero = 1; numero <= 10; numero++)
            {
                var questao = new Questao($"Quanto é {numero}+{numero} ?", materia);

                questao.AdicionarAlternativa(new Alternativa('a', (numero + 1).ToString(), false));
                questao.AdicionarAlternativa(new Alternativa('b', (numero + 2).ToString(), true));
                questao.AdicionarAlternativa(new Alternativa('c', (numero + 3).ToString(), false));
                questao.AdicionarAlternativa(new Alternativa('d', (numero + 4).ToString(), false));

                dbContext.Questoes.Add(questao);
            }

            dbContext.SaveChanges();
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

            dbContext.Questoes.RemoveRange(dbContext.Questoes);

            dbContext.Testes.RemoveRange(dbContext.Testes);

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