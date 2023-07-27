using GeradorTestes.Dominio.ModuloMateria;

namespace GeradorTestes.Infra.Orm.ModuloMateria
{
    public class RepositorioMateriaEmOrm : RepositorioBaseEmOrm<Materia>, IRepositorioMateria
    {
        public RepositorioMateriaEmOrm(GeradorTestesDbContext dbContext) : base(dbContext)
        {
        }

        public Materia SelecionarPorNome(string nome)
        {
            return registros.FirstOrDefault(x => x.Nome == nome);
        }

        public List<Materia> SelecionarTodos(bool incluirDisciplina = false)
        {
            if (incluirDisciplina)
                return registros.Include(x => x.Disciplina).ToList();

            return registros.ToList();
        }
    }
}
