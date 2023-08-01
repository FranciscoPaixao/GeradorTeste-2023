using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Infra.Orm.ModuloQuestao
{
    public class RepositorioQuestaoEmOrm : RepositorioBaseEmOrm<Questao>, IRepositorioQuestao
    {
        public RepositorioQuestaoEmOrm(GeradorTestesDbContext dbContext) : base(dbContext)
        {
        }

        public Questao SelecionarPorId(Guid id, bool incluirAlternativas = false)
        {
            if (incluirAlternativas)
                return registros.Include(x => x.Alternativas).FirstOrDefault( x => x.Id == id);

            return registros.Find(id);
        }
    }
}
