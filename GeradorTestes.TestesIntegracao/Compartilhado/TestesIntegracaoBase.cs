using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GeradorTestes.TestesIntegracao.Compartilhado
{
    public class TestesIntegracaoBase
    {
        public TestesIntegracaoBase()
        {
            LimparTabelas();
        }

        protected static void LimparTabelas()
        {
            string? connectionString = ObterConnectionString();

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlLimpezaTabela =
                @"
                DELETE FROM [DBO].[TBQUESTAO]
                DBCC CHECKIDENT ('[TBQUESTAO]', RESEED, 0);

                DELETE FROM [DBO].[TBMATERIA]
                DBCC CHECKIDENT ('[TBMATERIA]', RESEED, 0);

                DELETE FROM [DBO].[TBDISCIPLINA]
                DBCC CHECKIDENT ('[TBDISCIPLINA]', RESEED, 0);";

            SqlCommand comando = new SqlCommand(sqlLimpezaTabela, sqlConnection);

            sqlConnection.Open();

            comando.ExecuteNonQuery();

            sqlConnection.Close();
        }

        protected static string? ObterConnectionString()
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