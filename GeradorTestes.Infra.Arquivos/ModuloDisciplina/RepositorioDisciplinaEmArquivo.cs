using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloDisciplina;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloDisciplina
{
    public class RepositorioDisciplinaEmArquivo : 
        RepositorioEmArquivoBase<Disciplina>, IRepositorioDisciplina
    {
        protected GeradorTesteJsonContext contextoDados;

        public RepositorioDisciplinaEmArquivo(IContextoPersistencia contexto)
        {
            contextoDados = contexto as GeradorTesteJsonContext;
        }

        public bool Existe(Disciplina registro)
        {
            throw new System.NotImplementedException();
        }

        public override List<Disciplina> ObterRegistros()
        {
            return contextoDados.Disciplinas;
        }

        public Disciplina SelecionarPorNome(string nome)
        {
            return ObterRegistros().FirstOrDefault(x => x.Nome == nome);   
        }

        public List<Disciplina> SelecionarTodos(bool incluirMaterias = false, bool incluirQuestoes = false)
        {
            return ObterRegistros();
        }

    }
}
