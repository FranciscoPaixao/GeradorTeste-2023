using GeradorTestes.Dominio.ModuloDisciplina;

namespace GeradorTestes.Infra.Orm.ModuloDisciplina
{
    public class RepositorioDisciplinaEmOrm : RepositorioBaseEmOrm<Disciplina>, IRepositorioDisciplina
    {
        public RepositorioDisciplinaEmOrm(GeradorTestesDbContext dbContext) : base(dbContext)
        {           
        }
      
        public Disciplina SelecionarPorNome(string nome)
        {
            return registros.FirstOrDefault(x => x.Nome == nome);
        }

        public List<Disciplina> SelecionarTodos(bool incluirMaterias = false, bool incluirQuestoes = false)
        {
            if (incluirMaterias && incluirQuestoes)
                return registros
                        .Include(d => d.Materias)
                        .ThenInclude(m => m.Questoes)
                        .ToList();

            else if (incluirMaterias)
                return registros
                        .Include(d => d.Materias)
                .ToList();

            return registros.ToList();
        }

    }
}
