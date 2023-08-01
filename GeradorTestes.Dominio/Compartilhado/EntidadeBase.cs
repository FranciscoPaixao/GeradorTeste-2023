using SequentialGuid;

namespace GeradorTestes.Dominio
{
    public abstract class EntidadeBase<T>
    {
        public Guid Id { get; set; }  
        
        public EntidadeBase()
        {
            Id = SequentialGuidGenerator.Instance.NewGuid();
        }

        public abstract void Atualizar(T registro);
    }
}