using FluentValidation.Results;

namespace GeradorTestes.Dominio.Compartilhado
{
    public interface IValidador<T> where T : EntidadeBase<T>
    {
        public ValidationResult Validate(T instance);
    }
}
