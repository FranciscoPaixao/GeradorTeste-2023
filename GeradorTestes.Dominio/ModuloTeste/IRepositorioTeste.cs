namespace GeradorTestes.Dominio.ModuloTeste
{
    public interface IRepositorioTeste : IRepositorio<Teste>
    {
        Teste SelecionarPorId(Guid id, bool incluirQuestoes = false, bool incluirAlternativas = false, bool incluirMateria = false);

        List<Teste> SelecionarTodos(bool incluirMateria = false, bool incluirDisciplina = false);
    }
}
