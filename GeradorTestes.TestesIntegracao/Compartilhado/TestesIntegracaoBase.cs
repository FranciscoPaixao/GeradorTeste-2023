using FizzWare.NBuilder;
using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Infra.Orm.Compartilhado;
using GeradorTestes.Infra.Orm.ModuloDisciplina;
using GeradorTestes.Infra.Orm.ModuloMateria;
using GeradorTestes.Infra.Orm.ModuloQuestao;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GeradorTestes.TestesIntegracao.Compartilhado
{
    public class TestesIntegracaoBase
    {
        protected IRepositorioDisciplina repositorioDisciplina;
        protected IRepositorioMateria repositorioMateria;
        protected IRepositorioQuestao repositorioQuestao;

        public TestesIntegracaoBase()
        {
            LimparTabelas();

            string connectionString = ObterConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<GeradorTestesDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new GeradorTestesDbContext(optionsBuilder.Options);

            repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);
            repositorioMateria = new RepositorioMateriaEmOrm(dbContext);
            repositorioQuestao = new RepositorioQuestaoEmOrm(dbContext);

            BuilderSetup.SetCreatePersistenceMethod<Disciplina>(repositorioDisciplina.Inserir);
            BuilderSetup.SetCreatePersistenceMethod<Materia>(repositorioMateria.Inserir);
            BuilderSetup.SetCreatePersistenceMethod<Questao>(repositorioQuestao.Inserir);
        }

        protected static void LimparTabelas()
        {
            string? connectionString = ObterConnectionString();

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlLimpezaTabela =
                @"
                DELETE FROM [DBO].[TBQUESTAO];
                DELETE FROM [DBO].[TBMATERIA];                
                DELETE FROM [DBO].[TBDISCIPLINA];";

            SqlCommand comando = new SqlCommand(sqlLimpezaTabela, sqlConnection);

            sqlConnection.Open();

            comando.ExecuteNonQuery();

            sqlConnection.Close();
        }

        protected static string ObterConnectionString()
        {
            var configuracao = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");
            return connectionString;
        }
    }
}