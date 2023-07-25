using GeradorTestes.Dominio;
using GeradorTestes.Dominio.ModuloMateria;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloMateria
{
    public class RepositorioMateriaEmArquivo : RepositorioEmArquivoBase<Materia>, IRepositorioMateria
    {
        protected GeradorTesteJsonContext contextoDados;

        public RepositorioMateriaEmArquivo(IContextoPersistencia contexto)
        {
            contextoDados = contexto as GeradorTesteJsonContext;
        }

        public bool Existe(Materia registro)
        {
            throw new System.NotImplementedException();
        }

        public override List<Materia> ObterRegistros()
        {
            return contextoDados.Materias;
        }

        public Materia SelecionarPorNome(string nome)
        {
            return ObterRegistros().FirstOrDefault(x => x.Nome == nome);
        }

        public List<Materia> SelecionarTodos(bool incluirDisciplina = false)
        {
            return ObterRegistros();
        }
    }
}
