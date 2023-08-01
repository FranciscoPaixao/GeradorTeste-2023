using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Dominio.ModuloTeste
{

    public class Teste : EntidadeBase<Teste>
    {
        public Teste()
        {
            Questoes = new List<Questao>();
        }

        public Teste(string titulo, bool provao, DateTime dataGeracao, int quantidadeQuestoes) : this()
        {
            Titulo = titulo;
            Provao = provao;
            DataGeracao = dataGeracao;
            QuantidadeQuestoes = quantidadeQuestoes;
        }

        public Teste(Guid id, string titulo, bool provao, DateTime dataGeracao, int quantidadeQuestoes) : this(titulo, provao, dataGeracao, quantidadeQuestoes) 
        {
            Id = id;
        }

        public string Titulo { get; set; }

        public List<Questao> Questoes { get; set; }

        public bool Provao { get; set; }

        public DateTime DataGeracao { get; set; }

        public Disciplina Disciplina { get; set; }

        public Materia Materia { get; set; }        

        public int QuantidadeQuestoes { get; set; }

        public bool QuestoesSorteadas { get; set; }

        #region Periodos em Texto
        public string ObterPeriodoEmTexto()
        {
            return new PeriodoEmTexto(DateTime.Now, DataGeracao).ObterPeriodo();
        }
        #endregion

        public Gabarito ObterGabarito()
        {
            Gabarito gabarito = new Gabarito();

            gabarito.AlternativasCorretas = new List<Alternativa>(QuantidadeQuestoes);

            foreach (var questao in Questoes)
            {
                Alternativa alternativa = questao.ObtemAlternativaCorreta();

                gabarito.AdicionaQuestaoCorreta(alternativa);
            }

            return gabarito;
        }

        public void SortearQuestoes()
        {
            QuestoesSorteadas = true;

            List<Questao> questoesSelecionadas = Provao ? Disciplina.ObterTodasQuestoes() : Materia.Questoes;

            if (questoesSelecionadas == null)
            {
                Questoes = new List<Questao>();
                return;
            }

            if (questoesSelecionadas.Count >= QuantidadeQuestoes)
                Questoes = questoesSelecionadas.Randomize(QuantidadeQuestoes).ToList();
            else
                Questoes = questoesSelecionadas.Randomize().ToList();
        }

        public override void Atualizar(Teste teste)
        {
            Titulo = teste.Titulo;
            Provao = teste.Provao;
            DataGeracao = teste.DataGeracao;
            Disciplina = teste.Disciplina;
            Materia = teste.Materia;
            QuantidadeQuestoes = teste.QuantidadeQuestoes;
            Questoes = teste.Questoes;
        }

        public void RemoverQuestoes()
        {
            Questoes.Clear();
        }

        /*
        public string[] Validar()
        {
            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(Titulo))
                erros.Add($"O 'título' do teste deve estar preenchido");

            if (Titulo.Length <= 2)
                erros.Add($"O 'título' do teste deve ter mais de 3 letras");
           
            if (Disciplina == null)
                erros.Add($"A 'disciplina' do teste deve estar preenchida");

            if (DataGeracao == DateTime.MinValue)
                erros.Add($"A 'data' do teste deve estar preenchida");

            if (Provao == false && Materia == null)
            {
                erros.Add($"A 'matéria' do teste deve estar preenchida");
            }

            if (Provao)
            {
                if (Materia != null && Materia.Questoes.Count < 1)
                    erros.Add("Matéria deve ter no mínimo uma questão");
            }

            if (QuantidadeQuestoes <= 1)
                erros.Add("A quantidade de questões deve ser maior que 1");

            if (QuestoesSorteadas == false && Questoes.Count <= 1)
                erros.Add("Deve ser sorteado questões para o teste");

            if (Questoes.Count < QuantidadeQuestoes) 
            {
                erros.Add("Não tem questões suficiente para realizar o teste");
            }

            return erros.ToArray();
        }
        */


    }
}