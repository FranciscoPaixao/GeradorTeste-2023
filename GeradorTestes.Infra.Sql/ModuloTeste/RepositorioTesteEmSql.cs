using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;
using GeradorTestes.Infra.Sql.ModuloQuestao;

namespace GeradorTestes.Infra.Sql.ModuloTeste
{
    public class RepositorioTesteEmSql :
        RepositorioEmSqlBase<Teste, MapeadorTesteSql>, IRepositorioTeste
    {
        public RepositorioTesteEmSql(string connectionString) : base(connectionString)
        {

        }

        #region sql queries
        protected override string sqlInserir =>
          @"INSERT INTO [TBTESTE]
                (
                    [TITULO],                    
                    [DATAGERACAO],
                    [QUANTIDADEQUESTOES],
                    [PROVAO],
                    [MATERIA_ID],
                    [DISCIPLINA_ID]
	            )
	            VALUES
                (
                    @TITULO,
                    @DATAGERACAO,
                    @QUANTIDADEQUESTOES,
                    @PROVAO,
                    @MATERIA_ID,
                    @DISCIPLINA_ID

                );";

        protected override string sqlExcluir =>
           @"DELETE FROM [TBTESTE]

		            WHERE
			            [ID] = @ID";

        protected override string sqlEditar => "";

        private string sqlAdicionarQuestao =>
          @"INSERT INTO [TBTESTE_TBQUESTAO]
                (
                    [TESTE_ID],                    
                    [QUESTAO_ID]
	            )
	            VALUES
                (
                    @TESTE_ID,
                    @QUESTAO_ID

                );";

        private string sqlRemoverQuestao =>
          @"DELETE FROM [TBTESTE_TBQUESTAO]

		            WHERE
			            [TESTE_ID] = @TESTE_ID;";

        protected override string sqlSelecionarTodos =>
             @"SELECT        
	                T.ID                    TESTE_ID
	               ,T.TITULO                TESTE_TITULO
	               ,T.DATAGERACAO           TESTE_DATAGERACAO
	               ,T.PROVAO                TESTE_PROVAO
 	               ,T.QUANTIDADEQUESTOES    TESTE_QUANTIDADEQUESTOES
                   
	               ,D.ID                    DISCIPLINA_ID
	               ,D.NOME                  DISCIPLINA_NOME
	               
	               ,M.ID                    MATERIA_ID
	               ,M.NOME                  MATERIA_NOME
	               ,M.SERIE                 MATERIA_SERIE 	

                FROM  
	                TBTESTE T 
                    
                    INNER JOIN TBDISCIPLINA D
                        ON T.DISCIPLINA_ID = D.ID

                    LEFT JOIN TBMATERIA M 
                        ON T.MATERIA_ID = M.ID";

        protected override string sqlSelecionarPorId =>
             @"SELECT        
	                T.ID                    TESTE_ID
	               ,T.TITULO                TESTE_TITULO
	               ,T.DATAGERACAO           TESTE_DATAGERACAO
	               ,T.PROVAO                TESTE_PROVAO
 	               ,T.QUANTIDADEQUESTOES    TESTE_QUANTIDADEQUESTOES
                   
	               ,D.ID                    DISCIPLINA_ID
	               ,D.NOME                  DISCIPLINA_NOME
	               
	               ,M.ID                    MATERIA_ID
	               ,M.NOME                  MATERIA_NOME
	               ,M.SERIE                 MATERIA_SERIE 	

                FROM  
	                TBTESTE T 
                    
                    INNER JOIN TBDISCIPLINA D
                        ON T.DISCIPLINA_ID = D.ID

                    LEFT JOIN TBMATERIA M 
                        ON T.MATERIA_ID = M.ID

                WHERE 
	                T.[ID] = @ID";

        private string sqlSelecionarQuestoesDoTeste =>
            @"SELECT 
	                Q.ID            QUESTAO_ID
	               ,Q.ENUNCIADO     QUESTAO_ENUNCIADO
	               ,Q.JAUTILIZADA   QUESTAO_JAUTILIZADA

	               ,M.ID            MATERIA_ID
	               ,M.NOME          MATERIA_NOME
                   ,M.SERIE         MATERIA_SERIE
                   
                FROM 
	                TBQUESTAO AS Q 

                INNER JOIN TBTESTE_TBQUESTAO AS TQ
                    ON Q.ID = TQ.QUESTAO_ID

                INNER JOIN TBMATERIA M 
                    ON Q.MATERIA_ID = M.ID                    

                WHERE 
	                TQ.TESTE_ID = @TESTE_ID";

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

        protected override string sqlExisteRegistro =>
             @"SELECT 

		            COUNT(*)

	            FROM 
		            [TBTESTE]

		        WHERE
                    [ID] = @ID";

        #endregion

        public override void Inserir(Teste teste)
        {
            base.Inserir(teste);

            AdicionarQuestoesNoTeste(teste);
        }
        
        public override void Excluir(Teste testeSelecionado)
        {
            RemoverQuestoesNoTeste(testeSelecionado);

            base.Excluir(testeSelecionado);
        }

        public override void Editar(Teste registro) { }

        public Teste SelecionarPorId(Guid id, bool incluirQuestoes = false, bool incluirAlternativas = false)
        {
            Teste teste = base.SelecionarPorId(id);

            if (teste == null)
                return null;            

            if (incluirQuestoes)
            {
                CarregarQuestoes(teste);

                if (incluirAlternativas)
                {
                    foreach (Questao questao in teste.Questoes)
                    {
                        CarregarAlternativas(questao);
                    }
                }
            }

            return teste;
        }

        private void AdicionarQuestoesNoTeste(Teste teste)
        {
            foreach (var questao in teste.Questoes)
            {
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("TESTE_ID", teste.Id),
                    new SqlParameter("QUESTAO_ID", questao.Id)
                };

                ExecutarComando(sqlAdicionarQuestao, parametros);
            }
        }

        private void CarregarQuestoes(Teste teste)
        {
            MapeadorQuestaoSql mapeador = new MapeadorQuestaoSql();

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("TESTE_ID", teste.Id)
            };

            List<Questao> questoes = SelecionarRegistros(sqlSelecionarQuestoesDoTeste, mapeador.ConverterRegistro, parametros);

            foreach (Questao questao in questoes)
            {
                teste.Questoes.Add(questao);
            }
        }

        private void RemoverQuestoesNoTeste(Teste teste)
        {
            SqlParameter[] parametros = new SqlParameter[]
               {
                    new SqlParameter("TESTE_ID", teste.Id),
               };

            ExecutarComando(sqlRemoverQuestao, parametros);
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

        public List<Teste> SelecionarTodos(bool incluirMateria = false, bool incluirDisciplina = false)
        {
            throw new NotImplementedException();
        }

        public Teste SelecionarPorId(Guid id, bool incluirQuestoes = false, bool incluirAlternativas = false, bool incluirMateria = false)
        {
            throw new NotImplementedException();
        }
    }
}
