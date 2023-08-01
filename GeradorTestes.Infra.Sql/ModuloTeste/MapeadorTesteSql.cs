using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloTeste;
using GeradorTestes.Infra.Sql.ModuloDisciplina;
using GeradorTestes.Infra.Sql.ModuloMateria;

namespace GeradorTestes.Infra.Sql.ModuloTeste
{
    public class MapeadorTesteSql : MapeadorBase<Teste>
    {
        public override void ConfigurarParametros(SqlCommand comando, Teste teste)
        {
            comando.Parameters.AddWithValue("ID", teste.Id);

            comando.Parameters.AddWithValue("TITULO", teste.Titulo);

            comando.Parameters.AddWithValue("DATAGERACAO", teste.DataGeracao);

            comando.Parameters.AddWithValue("QUANTIDADEQUESTOES", teste.QuantidadeQuestoes);

            comando.Parameters.AddWithValue("DISCIPLINA_ID", teste.Disciplina.Id);

            if (teste.Materia != null)
                comando.Parameters.AddWithValue("MATERIA_ID", teste.Materia.Id);
            else
                comando.Parameters.AddWithValue("MATERIA_ID", DBNull.Value);

            comando.Parameters.AddWithValue("PROVAO", teste.Provao);
        }

        public override Teste ConverterRegistro(SqlDataReader leitorTeste)
        {
            Guid id = Guid.Parse(leitorTeste["TESTE_ID"].ToString());

            string titulo = Convert.ToString(leitorTeste["TESTE_TITULO"]);

            bool provao = Convert.ToBoolean(leitorTeste["TESTE_PROVAO"]);

            DateTime dataGeracao = Convert.ToDateTime(leitorTeste["TESTE_DATAGERACAO"]);

            int qtdQuestoes = Convert.ToInt32(leitorTeste["TESTE_QUANTIDADEQUESTOES"]);

            Disciplina disciplina = new MapeadorDisciplinaSql().ConverterRegistro(leitorTeste);

            Teste teste = new Teste(id, titulo, provao, dataGeracao, qtdQuestoes);

            if (provao)
            {
                teste.Disciplina = disciplina;
            }
            else
            {
                Materia materia = new MapeadorMateriaSql().ConverterRegistro(leitorTeste);
                materia.Disciplina = disciplina;
                teste.Materia = materia;
            }

            return teste;
        }
    }
}