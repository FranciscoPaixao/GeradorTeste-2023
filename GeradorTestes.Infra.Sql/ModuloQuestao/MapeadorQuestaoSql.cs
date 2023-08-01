using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Infra.Sql.ModuloDisciplina;
using GeradorTestes.Infra.Sql.ModuloMateria;

namespace GeradorTestes.Infra.Sql.ModuloQuestao
{
    public class MapeadorQuestaoSql : MapeadorBase<Questao>
    {
        public override void ConfigurarParametros(SqlCommand comando, Questao questao)
        {
            comando.Parameters.AddWithValue("ID", questao.Id);

            comando.Parameters.AddWithValue("ENUNCIADO", questao.Enunciado);

            comando.Parameters.AddWithValue("JAUTILIZADA", questao.JaUtilizada);

            comando.Parameters.AddWithValue("MATERIA_ID", questao.Materia.Id);
        }

        public override Questao ConverterRegistro(SqlDataReader leitorQuestao)
        {
            Disciplina disciplina = new MapeadorDisciplinaSql().ConverterRegistro(leitorQuestao);

            Materia materia = new MapeadorMateriaSql().ConverterRegistro(leitorQuestao);

            if (materia != null && disciplina != null)
                materia.Disciplina = disciplina;

            Guid id = Guid.Parse(leitorQuestao["QUESTAO_ID"].ToString());

            string enunciado = Convert.ToString(leitorQuestao["QUESTAO_ENUNCIADO"]);

            bool jaUtilizada = Convert.ToBoolean(leitorQuestao["QUESTAO_JAUTILIZADA"]);

            Questao questao = new Questao(id, enunciado, materia, jaUtilizada);

            return questao;
        }
    }
}