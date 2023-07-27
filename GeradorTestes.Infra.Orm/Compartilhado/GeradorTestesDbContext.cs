using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace GeradorTestes.Infra.Orm.Compartilhado
{
    public class GeradorTestesDbContext : DbContext
    {
        /* Para criar objetos DbContext o padrão é utilizar a classe "DbContextOptions"
         * 
         * O código abaixo é uma "gambeta" :-)             
         * 
         * private string connectionString;
         * 
         * public GeradorTestesDbContext(string connectionString)
         * {
         *     this.connectionString = connectionString;
         * }
        */
        public GeradorTestesDbContext(DbContextOptions options) : base(options)
        {            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /* Para criar objetos DbContext o padrão é utilizar a classe "DbContextOptions"
             * 
             * O código abaixo é uma "gambeta" :-)             
             * 
             * if (string.IsNullOrEmpty(connectionString))
             * {
             *     var configuration = new ConfigurationBuilder()
             *        .SetBasePath(Directory.GetCurrentDirectory())
             *        .AddJsonFile("appsettings.json")
             *        .Build();
             * 
             *     connectionString = configuration.GetConnectionString("SqlServer");
             * }            
             * 
             * optionsBuilder.UseSqlServer(connectionString);
             *
            */

            ILoggerFactory loggerFactory = LoggerFactory.Create((x) =>
            {
                x.AddSerilog(Log.Logger); //instalar o pacote Serilog.Extensions.Logging
            });

            optionsBuilder.UseLoggerFactory(loggerFactory);

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Registro dos mapeadores de forma manual
            modelBuilder.ApplyConfiguration(new MapeadorDisciplinaOrm());

            modelBuilder.ApplyConfiguration(new MapeadorMateriaOrm());

            modelBuilder.ApplyConfiguration(new MapeadorQuestaoOrm());

            modelBuilder.ApplyConfiguration(new MapeadorAlternativaOrm());

            modelBuilder.ApplyConfiguration(new MapeadorTesteOrm());
            */

            Assembly assembly = typeof(GeradorTestesDbContext).Assembly;
            
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}