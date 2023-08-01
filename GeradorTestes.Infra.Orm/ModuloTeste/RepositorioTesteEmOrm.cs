using GeradorTestes.Dominio.ModuloTeste;

namespace GeradorTestes.Infra.Orm.ModuloTeste
{
    public class RepositorioTesteEmOrm : RepositorioBaseEmOrm<Teste>, IRepositorioTeste
    {
        public RepositorioTesteEmOrm(GeradorTestesDbContext dbContext) : base(dbContext)
        {
        }

        public Teste SelecionarPorId(Guid id, bool incluirQuestoes = false, bool incluirAlternativas = false, bool incluirMateria = false)
        {
            if (incluirQuestoes && incluirAlternativas)
                return registros
                    .Include(x => x.Questoes)
                    .ThenInclude(x => x.Alternativas)
                    .FirstOrDefault(x => x.Id == id);

            else if (incluirQuestoes)
                return registros
                    .Include(x => x.Questoes)
                    .FirstOrDefault(x => x.Id == id);

            return registros.FirstOrDefault(x => x.Id == id);
        }

        public List<Teste> SelecionarTodos(bool incluirMateria = false, bool incluirDisciplina = false)
        {
            if (incluirMateria && incluirDisciplina)
                return registros
                    .Include(x => x.Materia)
                    .Include(x => x.Disciplina)
                    .ToList();

            else if (incluirMateria)
                return registros
                    .Include(x => x.Materia)
                    .ToList();

            else if (incluirDisciplina)
                return registros
                    .Include(x => x.Disciplina)
                    .ToList();

            return registros.ToList();
        }
    }
}
