namespace GeradorTestes.Dominio
{
    public interface IContextoPersistencia // Unit of Work - UoW
    {
        void DesfazerAlteracoes();

        void GravarDados();
    }
}
