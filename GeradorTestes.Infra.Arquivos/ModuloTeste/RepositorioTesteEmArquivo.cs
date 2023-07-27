using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloTeste;
using System.Collections.Generic;

namespace eAgenda.Infra.Arquivos.ModuloTeste
{
    public class RepositorioTesteEmArquivo : RepositorioEmArquivoBase<Teste>, IRepositorioTeste
    {
        protected GeradorTesteJsonContext contextoDados;

        public RepositorioTesteEmArquivo(IContextoPersistencia contexto)
        {
            contextoDados = contexto as GeradorTesteJsonContext;
        }

        public bool Existe(Teste registro)
        {
            throw new System.NotImplementedException();
        }

        public override List<Teste> ObterRegistros()
        {
            return contextoDados.Testes;
        }

        public Teste SelecionarPorId(int id, bool incluirQuestoes = false, bool incluirAlternativas = false)
        {
            return base.SelecionarPorId(id);
        }

        public Teste SelecionarPorId(int id, bool incluirQuestoes = false, bool incluirAlternativas = false, bool incluirMateria = false)
        {
            throw new System.NotImplementedException();
        }

        public List<Teste> SelecionarTodos(bool incluirDisciplinaEhMateria)
        {
            return ObterRegistros();
        }

        public List<Teste> SelecionarTodos(bool incluirMateria = false, bool incluirDisciplina = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
