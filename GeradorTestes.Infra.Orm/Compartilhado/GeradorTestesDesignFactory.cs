using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GeradorTestes.Infra.Orm.Compartilhado
{
    internal class GeradorTestesDesignFactory : IDesignTimeDbContextFactory<GeradorTestesDbContext>
    {
        public GeradorTestesDbContext CreateDbContext(string[] args)
        {            
            var configuracao = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var optionsBuilder = new DbContextOptionsBuilder<GeradorTestesDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new GeradorTestesDbContext(optionsBuilder.Options);
        }
    }
}
