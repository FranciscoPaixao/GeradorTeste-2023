using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloQuestao;
using System.Collections.Generic;

namespace eAgenda.Infra.Arquivos.ModuloQuestao
{
    public class RepositorioQuestaoEmArquivo : RepositorioEmArquivoBase<Questao>, IRepositorioQuestao
    {
        protected GeradorTesteJsonContext contextoDados;

        public RepositorioQuestaoEmArquivo(IContextoPersistencia contexto)
        {
            contextoDados = contexto as GeradorTesteJsonContext;
        }

        public bool Existe(Questao registro)
        {
            throw new System.NotImplementedException();
        }

        public override List<Questao> ObterRegistros()
        {
            return contextoDados.Questoes;
        }

        public Questao SelecionarPorId(int id, bool incluirAlternativas = false)
        {
            return SelecionarPorId(id);
        }       
    }
}
