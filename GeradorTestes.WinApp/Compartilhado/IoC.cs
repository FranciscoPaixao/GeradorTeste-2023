namespace GeradorTestes.WinApp.Compartilhado
{
    public interface IoC
    {
        ControladorBase Get<T>();
    }
}
