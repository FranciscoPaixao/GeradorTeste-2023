using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Infra.Sql.ModuloQuestao
{
    public class RepositorioQuestaoEmSql : RepositorioEmSqlBase<Questao, MapeadorQuestaoSql>, IRepositorioQuestao
    {
        public RepositorioQuestaoEmSql(string connectionString) : base(connectionString)
        {

        }
        protected override string sqlInserir =>
             @"INSERT INTO [TBQUESTAO]
                (
                    [ENUNCIADO]
                   ,[JAUTILIZADA]
                   ,[MATERIA_ID]
	            )
	            VALUES
                (
                    @ENUNCIADO
                   ,@JAUTILIZADA
                   ,@MATERIA_ID
                );";

        protected override string sqlEditar =>
            @"UPDATE [TBQUESTAO]

		            SET                    
                        [ENUNCIADO] = @ENUNCIADO
                       ,[JAUTILIZADA] = @JAUTILIZADA
                       ,[MATERIA_ID] = @MATERIA_ID

		            WHERE
			            [ID] = @ID";

        protected override string sqlExcluir =>
            @"DELETE FROM [TBQUESTAO]

		            WHERE
			            [ID] = @ID";

        protected override string sqlSelecionarTodos =>
            @"SELECT 
	                Q.ID            QUESTAO_ID                
	               ,Q.ENUNCIADO     QUESTAO_ENUNCIADO
	               ,Q.JAUTILIZADA   QUESTAO_JAUTILIZADA
                   
	               ,M.ID            MATERIA_ID
	               ,M.NOME          MATERIA_NOME
                   ,M.SERIE         MATERIA_SERIE
                   
	               ,D.ID            DISCIPLINA_ID
	               ,D.NOME          DISCIPLINA_NOME

                FROM
	                TBQUESTAO Q 
    
                    INNER JOIN TBMATERIA M 
                        ON Q.MATERIA_ID = M.ID
                    
                    INNER JOIN TBDISCIPLINA D 
                        ON D.ID = M.DISCIPLINA_ID";

        protected override string sqlSelecionarPorId =>
            @"SELECT 
	                Q.ID            QUESTAO_ID                
	               ,Q.ENUNCIADO     QUESTAO_ENUNCIADO
	               ,Q.JAUTILIZADA   QUESTAO_JAUTILIZADA
                   
	               ,M.ID            MATERIA_ID
	               ,M.NOME          MATERIA_NOME
                   ,M.SERIE         MATERIA_SERIE
                   
	               ,D.ID            DISCIPLINA_ID
	               ,D.NOME          DISCIPLINA_NOME

                FROM
	                TBQUESTAO Q 
    
                    INNER JOIN TBMATERIA M 
                        ON Q.MATERIA_ID = M.ID
                    
                    INNER JOIN TBDISCIPLINA D 
                        ON D.ID = M.DISCIPLINA_ID

                WHERE 
                    Q.ID = @ID";

        private string sqlInserirAlternativas =>
           @"INSERT INTO [TBALTERNATIVA]
                    (
		                [QUESTAO_ID]
		               ,[LETRA]
		               ,[RESPOSTA]
		               ,[CORRETA]
	                )
                     VALUES
                    (
		                @QUESTAO_ID
		               ,@LETRA
		               ,@RESPOSTA
		               ,@CORRETA
	                );";

        private string sqlSelecionarAlternativas =>
           @"SELECT 
	                [ID]            ALTERNATIVA_ID                   
                   ,[LETRA]         ALTERNATIVA_LETRA
                   ,[RESPOSTA]      ALTERNATIVA_RESPOSTA
                   ,[CORRETA]       ALTERNATIVA_CORRETA

                  FROM 
	                    [TBALTERNATIVA]

                  WHERE 
	                    [QUESTAO_ID] = @QUESTAO_ID";

        private string sqlExcluirAlternativas =>
           @"DELETE FROM [TBALTERNATIVA]

		            WHERE
			            [QUESTAO_ID] = @QUESTAO_ID";

        protected override string sqlExisteRegistro =>
             @"SELECT 

		            COUNT(*)

	            FROM 
		            [TBQUESTAO]

		        WHERE
                    [ID] = @ID";

        public override void Inserir(Questao questao)
        {
            base.Inserir(questao);

            AdicionarAlternativas(questao);
        }

        public override void Editar(Questao questao)
        {
            base.Editar(questao);

            RemoverAlternativas(questao);

            AdicionarAlternativas(questao);
        }

        public override void Excluir(Questao questaoSelecionada)
        {
            RemoverAlternativas(questaoSelecionada);

            base.Excluir(questaoSelecionada);
        }

        public Questao SelecionarPorId(Guid id, bool incluirAlternativas = false)
        {
            Questao questao = base.SelecionarPorId(id);

            if (incluirAlternativas)
                CarregarAlternativas(questao);

            return questao;
        }

        private void AdicionarAlternativas(Questao questao)
        {
            foreach (Alternativa alternativa in questao.Alternativas)
            {
                MapeadorAlternativa mapeador = new MapeadorAlternativa();

                ExecutarComando(sqlInserirAlternativas, mapeador.ObterParametros(alternativa));
            }
        }

        private void CarregarAlternativas(Questao questao)
        {
            MapeadorAlternativa mapeador = new MapeadorAlternativa();

            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("QUESTAO_ID", questao.Id) };

            List<Alternativa> alternativas = SelecionarRegistros(sqlSelecionarAlternativas, mapeador.ConverterRegistro, parametros);

            foreach (Alternativa alternativa in alternativas)
            {
                questao.AdicionarAlternativa(alternativa);
            }
        }

        private void RemoverAlternativas(Questao questao)
        {
            SqlParameter[] parametros = new SqlParameter[] { new SqlParameter("QUESTAO_ID", questao.Id) };

            ExecutarComando(sqlExcluirAlternativas, parametros);
        }
    }
}
