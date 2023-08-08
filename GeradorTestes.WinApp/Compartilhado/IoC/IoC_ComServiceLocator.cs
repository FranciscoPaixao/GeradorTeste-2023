namespace GeradorTestes.WinApp.Compartilhado
{
    /* Exemplo sem Injeção de Dependência
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
    */
}
