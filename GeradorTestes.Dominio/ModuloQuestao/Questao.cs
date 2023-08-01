using GeradorTestes.Dominio.ModuloMateria;

namespace GeradorTestes.Dominio.ModuloQuestao
{
    public class Questao : EntidadeBase<Questao>
    {

        public Questao()
        {
            Alternativas = new List<Alternativa>();
        }

        public Questao(string enunciado, Materia materia) : this()
        {
            Enunciado = enunciado;
            Materia = materia;
            JaUtilizada = false;
        }

        public Questao(string enunciado, Materia materia, bool jaUtilizada) : this()
        {
            Enunciado = enunciado;
            Materia = materia;
            JaUtilizada = jaUtilizada;
        }

        public Questao(Guid id, string enunciado, Materia materia, bool jaUtilizada) : this(enunciado, materia, jaUtilizada)
        {
            Id = id;
        }

        public List<Alternativa> Alternativas { get; set; }

        public string Enunciado { get; set; }

        public Materia Materia { get; set; }

        public bool JaUtilizada { get; set; }

        public bool AdicionarAlternativa(Alternativa alternativa)
        {
            if (Alternativas.Contains(alternativa))
                return false;

            alternativa.Questao = this;
            Alternativas.Add(alternativa);

            return true;
        }

        public void AtualizarAlternativaCorreta(Alternativa alternativaCorreta)
        {
            foreach (var a in Alternativas)
            {
                if (a.Equals(alternativaCorreta))
                    a.Correta = true;                
            }
        }

        public void RemoverAlternativa(Alternativa alternativa)
        {
            Alternativas.Remove(alternativa);

            RedefinirLetras();
        }

        public Alternativa ObtemAlternativaCorreta()
        {
            if (Alternativas.Any())
                return Alternativas.FirstOrDefault(x => x.Correta);

            return null;
        }

        public override void Atualizar(Questao questao)
        {
            Enunciado = questao.Enunciado;
            Materia = questao.Materia;
        }

        private void RedefinirLetras()
        {
            char letra = 'A';

            foreach (var item in Alternativas)
            {
                item.Letra = letra;
                letra = letra.Next();
            }
        }

        public char GerarLetraAlternativa()
        {
            if (Alternativas == null)
                Alternativas = new List<Alternativa>();

            return Alternativas.Count == 0 ? 'A' :
                Alternativas.Select(x => x.Letra).Last().Next();
        }

        public void ConfigurarMateria(Materia materia)
        {
            if (materia == null)
                return;

            Materia = materia;
            Materia.AdicionaQuestao(this);
        }

        public override string ToString()
        {
            return Enunciado;
        }

        public override bool Equals(object obj)
        {
            return obj is Questao questao &&
                   Id == questao.Id &&                   
                   Enunciado == questao.Enunciado &&
                   Materia.Equals(questao.Materia) &&
                   JaUtilizada == questao.JaUtilizada;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Alternativas, Enunciado, Materia, JaUtilizada);
        }



        /*
        public string[] Validar()
        {
            List<string> erros = new List<string>();

            if (Materia == null)
                erros.Add($"A 'matéria' da questão deve estar preenchida");

            if (string.IsNullOrEmpty(Enunciado))
                erros.Add($"O 'enunciado' da questão deve estar preenchido");

            if (Enunciado.Length <= 2)
                erros.Add($"O 'enunciado' da questão deve ter mais de 3 letras");                       

            foreach (Alternativa a in Alternativas)
            {
                if (string.IsNullOrEmpty(a.Resposta))
                {
                    erros.Add($"As 'respostas das alternativas' devem estar preenchidas");
                    break;
                }
            }

            if (Alternativas.Count(x => x.Correta) == 0)
                erros.Add("Nenhuma alternativa correta foi informada");

            if (Alternativas.Count() < 3)
                erros.Add("No mínimo 3 alternativas precisa ser informada");

            if (Alternativas.Count() > 5)
                erros.Add("No máximo 5 alternativas deve ser informada");

            if (Alternativas.Count(x => x.Correta) > 1)
                erros.Add("Apenas uma alternativa pode ser correta");

            if (AlternativasComValoresDuplicados())
                erros.Add("Respostas iguais foram informadas nas alternativas");

            return erros.ToArray();
        }

        private bool AlternativasComValoresDuplicados()
        {
            string[] respostas = Alternativas.Select(a => a.Resposta).ToArray();

            Dictionary<string, int> dicionario = new Dictionary<string, int>();

            foreach (var value in respostas)
            {
                dicionario.TryGetValue(value, out int count);
                dicionario[value] = count + 1;
            }

            if (dicionario.Values.Any(x => x > 1))
                return true;

            return false;
        }
        */
    }
}