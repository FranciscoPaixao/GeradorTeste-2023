using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Infra.Sql.ModuloQuestao
{
    public class MapeadorAlternativa 
    {
        public SqlParameter[] ObterParametros(Alternativa alternativa)
        {
            List<SqlParameter> parametros = new List<SqlParameter>() 
            {
                new SqlParameter("ID", alternativa.Id),
                new SqlParameter("LETRA", alternativa.Letra),
                new SqlParameter("RESPOSTA", alternativa.Resposta),
                new SqlParameter("CORRETA", alternativa.Correta),
                new SqlParameter("QUESTAO_ID", alternativa.Questao.Id)
            };

            return parametros.ToArray();
        }

        public Alternativa ConverterRegistro(SqlDataReader leitorAlternativa)
        {
            Guid id = Guid.Parse(leitorAlternativa["ALTERNATIVA_ID"].ToString());

            char letra = Convert.ToChar(leitorAlternativa["ALTERNATIVA_LETRA"]);

            string resposta = Convert.ToString(leitorAlternativa["ALTERNATIVA_RESPOSTA"]);

            bool correta = Convert.ToBoolean(leitorAlternativa["ALTERNATIVA_CORRETA"]);

            Alternativa alternativa = new Alternativa(id, letra, resposta, correta);

            return alternativa;
        }
    }
}
