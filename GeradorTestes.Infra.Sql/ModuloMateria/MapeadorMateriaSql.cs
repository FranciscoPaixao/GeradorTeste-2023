using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Infra.Sql.ModuloDisciplina;

namespace GeradorTestes.Infra.Sql.ModuloMateria
{
    public class MapeadorMateriaSql : MapeadorBase<Materia>
    {
        public override void ConfigurarParametros(SqlCommand comando, Materia materia)
        {
            comando.Parameters.AddWithValue("ID", materia.Id);

            comando.Parameters.AddWithValue("NOME", materia.Nome);

            comando.Parameters.AddWithValue("SERIE", materia.Serie);

            comando.Parameters.AddWithValue("DISCIPLINA_ID", materia.Disciplina.Id);
        }

        public override Materia ConverterRegistro(SqlDataReader leitorMateria)
        {
            if (leitorMateria.HasColumn("MATERIA_ID") == false)
                return null;

            Guid id = Guid.Parse(leitorMateria["MATERIA_ID"].ToString());

            string nome = Convert.ToString(leitorMateria["MATERIA_NOME"]);

            SerieMateriaEnum serie = (SerieMateriaEnum)leitorMateria["MATERIA_SERIE"];

            Disciplina disciplina = new MapeadorDisciplinaSql().ConverterRegistro(leitorMateria);

            Materia materia = new Materia(id, nome, serie, disciplina);

            return materia;
        }
    }
}