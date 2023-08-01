using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Dominio.ModuloDisciplina
{
    public class Disciplina : EntidadeBase<Disciplina>
    {
        public Disciplina()
        {
            Materias = new List<Materia>();
        }

        public Disciplina(string nome) : this()
        {
            Nome = nome;
        }

        public Disciplina(Guid id, string nome) : this(nome)        
        {
            this.Id = id;
        }

        public string Nome { get; set; }

        public List<Materia> Materias { get; set; }

        public List<Questao> ObterTodasQuestoes()
        {
            var todasQuestoes = new List<Questao>();

            if (Materias.Any())
            {
                foreach (var m in Materias)
                {
                    if (m.Questoes != null)
                        todasQuestoes.AddRange(m.Questoes);
                }
            }

            return todasQuestoes;
        }       

        public bool AdicionarMateria(Materia materia)
        {
            if (Materias.Contains(materia))
                return false;

            Materias.Add(materia);

            return true;
        }

        public override string ToString()
        {
            return Nome;
        }

        public override void Atualizar(Disciplina disciplina)
        {
            Nome = disciplina.Nome;
        }

        public override bool Equals(object obj)
        {
            return obj is Disciplina disciplina &&
                   Id == disciplina.Id &&
                   Nome == disciplina.Nome;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Materias, Nome);
        }       
    }
}