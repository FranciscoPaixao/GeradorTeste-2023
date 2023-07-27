namespace GeradorTestes.Dominio.ModuloMateria
{
    public interface IRepositorioMateria : IRepositorio<Materia>
    {
        Materia SelecionarPorNome(string nome);

        List<Materia> SelecionarTodos(bool incluirDisciplina = false);
    }
}