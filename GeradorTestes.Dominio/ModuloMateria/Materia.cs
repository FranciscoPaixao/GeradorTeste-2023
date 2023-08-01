using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Dominio.ModuloMateria
{
    public class Materia : EntidadeBase<Materia>
    {
        public Materia()
        {
            Questoes = new List<Questao>();
        }

        public Materia(string n, SerieMateriaEnum s, Disciplina disciplina) : this()
        {
            Nome = n;
            Serie = s;
            Disciplina = disciplina;
        }

        public Materia(Guid id, string nome, SerieMateriaEnum serie, Disciplina disciplina) : this(nome, serie, disciplina)
        {
            this.Id = id;
        }

        public string Nome { get; set; }

        public SerieMateriaEnum Serie { get; set; }

        public Disciplina Disciplina { get; set; }

        public List<Questao> Questoes { get; set; }

        public void AdicionaQuestao(Questao questao)
        {           
            if (Questoes.Contains(questao))
                return;

            Questoes.Add(questao);
            questao.Materia = this;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Nome, Serie.GetDescription());
        }
        
        public override void Atualizar(Materia materia)
        {
            Nome = materia.Nome;
            Disciplina = materia.Disciplina;
            Serie = materia.Serie;
        }

        public override bool Equals(object obj)
        {
            return obj is Materia materia &&
                   Id == materia.Id &&
                   Nome == materia.Nome &&
                   Serie == materia.Serie;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nome, Serie, Disciplina, Questoes);
        }

        /*
        public string[] Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(Nome))
                erros.Add($"O 'nome' da matéria deve estar preenchido");

            if (Nome.Length <= 2)
                erros.Add($"O 'nome' da matéria deve ter mais de 3 letras");

            bool temCaracteresInvalidos = false;

            foreach (char letra in Nome)
            {
                if (letra == ' ')
                    continue;

                if (char.IsLetterOrDigit(letra) == false)
                    temCaracteresInvalidos = true;
            }

            if (temCaracteresInvalidos)
                erros.Add($"O 'nome' da matéria deve ser composta por letras e números");

            if (Disciplina == null)
                erros.Add($"A 'disciplina' da matéria deve estar preenchida");

            if (Serie == SerieMateriaEnum.Nenhum)
                erros.Add($"A 'série' da matéria deve estar preenchida");

            return erros.ToArray();
        }
        */
    }
}