using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Infra.Sql.ModuloMateria;
using GeradorTestes.Infra.Sql.ModuloQuestao;

namespace GeradorTestes.Infra.Sql.ModuloDisciplina
{
    public class RepositorioDisciplinaEmSql :
        RepositorioEmSqlBase<Disciplina, MapeadorDisciplinaSql>, IRepositorioDisciplina
    {
        public RepositorioDisciplinaEmSql(string connectionString) : base(connectionString)
        {
            
        }

        #region slq queries
        protected override string sqlInserir =>
            @"INSERT INTO [TBDISCIPLINA]
                (
                    [NOME]
                )    
                 VALUES
                (
                    @NOME
                )";

        protected override string sqlEditar =>
            @"UPDATE [TBDISCIPLINA]	
		        SET
			        [NOME] = @NOME
		        WHERE
			        [ID] = @ID";

        protected override string sqlExcluir =>
            @"DELETE FROM [TBDISCIPLINA]
		        WHERE
			        [ID] = @ID";

        protected override string sqlSelecionarTodos =>
            @"SELECT 
		            [ID]    DISCIPLINA_ID
		           ,[NOME]  DISCIPLINA_NOME

	            FROM 
		            [TBDISCIPLINA]";

        protected override string sqlSelecionarPorId =>
            @"SELECT 
		            [ID]    DISCIPLINA_ID
		           ,[NOME]  DISCIPLINA_NOME

	            FROM 
		            [TBDISCIPLINA]

		        WHERE
                    [ID] = @ID";

        private string sqlSelecionarPorNome =>
            @"SELECT 
		            [ID]    DISCIPLINA_ID
		           ,[NOME]  DISCIPLINA_NOME

	            FROM 
		            [TBDISCIPLINA]

		        WHERE
                    [NOME] = @NOME";

        private string sqlSelecionarMateriasDaDisciplina =>
            @"SELECT 
		            [ID]        MATERIA_ID 
		           ,[NOME]      MATERIA_NOME
                   ,[SERIE]     MATERIA_SERIE

	            FROM 
		            [TBMATERIA]

		        WHERE
                    [DISCIPLINA_ID] = @DISCIPLINA_ID";

        private string sqlSelecionarQuestoesDaMateria =>
            @"SELECT 

		            [ID]            QUESTAO_ID
		           ,[ENUNCIADO]     QUESTAO_ENUNCIADO
	               ,[JAUTILIZADA]   QUESTAO_JAUTILIZADA                        

	            FROM 
		            [TBQUESTAO]

		        WHERE
                    [MATERIA_ID] = @MATERIA_ID AND [JAUTILIZADA] = 0";

        protected override string sqlExisteRegistro =>
             @"SELECT 

		            COUNT(*)

	            FROM 
		            [TBDISCIPLINA]

		        WHERE
                    [ID] = @ID";


        #endregion

        public List<Disciplina> SelecionarTodos(bool incluirMaterias = false, bool incluirQuestoes=false)
        {
            List<Disciplina> disciplinas = base.SelecionarTodos();

            if (incluirMaterias)
            {
                foreach (Disciplina disciplina in disciplinas)
                {
                    CarregarMaterias(disciplina);

                    if (incluirQuestoes)
                    {
                        foreach (Materia materia in disciplina.Materias)
                        {
                            CarregarQuestoes(materia);
                        }
                    }
                }
            }

            return disciplinas;
        }

        public Disciplina SelecionarPorNome(string nome)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("NOME", nome) };

            return base.SelecionarRegistroPorParametro(sqlSelecionarPorNome, parametros);
        }


        private void CarregarMaterias(Disciplina disciplina)
        {
            MapeadorMateriaSql mapeador = new MapeadorMateriaSql();

            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("DISCIPLINA_ID", disciplina.Id) };

            List<Materia> materias = SelecionarRegistros(sqlSelecionarMateriasDaDisciplina, mapeador.ConverterRegistro, parametros);

            foreach (Materia materia in materias)
            {
                disciplina.AdicionarMateria(materia);
            }
        }

        private void CarregarQuestoes(Materia materia)
        {
            MapeadorQuestaoSql mapeador = new MapeadorQuestaoSql();

            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("MATERIA_ID", materia.Id) };

            List<Questao> questoes = SelecionarRegistros(sqlSelecionarQuestoesDaMateria, mapeador.ConverterRegistro, parametros);

            foreach (Questao questao in questoes)
            {
                materia.AdicionaQuestao(questao);
            }
        }
    }
}