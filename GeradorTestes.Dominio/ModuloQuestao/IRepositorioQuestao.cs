namespace GeradorTestes.Dominio.ModuloQuestao
{
    public interface IRepositorioQuestao : IRepositorio<Questao>
    {
        Questao SelecionarPorId(Guid id, bool incluirAlternativas = false);
    }
}
