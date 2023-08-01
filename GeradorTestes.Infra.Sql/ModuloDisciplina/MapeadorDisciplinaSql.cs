using GeradorTestes.Dominio.ModuloDisciplina;

namespace GeradorTestes.Infra.Sql.ModuloDisciplina
{
    public class MapeadorDisciplinaSql : MapeadorBase<Disciplina>
    {
        public override void ConfigurarParametros(SqlCommand comando, Disciplina disciplina)
        {
            comando.Parameters.AddWithValue("ID", disciplina.Id);

            comando.Parameters.AddWithValue("NOME", disciplina.Nome);
        }

        public override Disciplina ConverterRegistro(SqlDataReader leitorDisciplina)
        {            
            if (leitorDisciplina.HasColumn("DISCIPLINA_ID") == false)
                return null;    

            Guid id = Guid.Parse(leitorDisciplina["DISCIPLINA_ID"].ToString());

            string nome = Convert.ToString(leitorDisciplina["DISCIPLINA_NOME"]);

            Disciplina disciplina = new Disciplina(id, nome);

            return disciplina;
        }
    }
}
