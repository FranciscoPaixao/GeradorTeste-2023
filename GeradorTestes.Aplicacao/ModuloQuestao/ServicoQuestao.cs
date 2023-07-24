using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Aplicacao.ModuloQuestao
{
    public class ServicoQuestao
    {
        private IRepositorioQuestao repositorioQuestao;
        private ValidadorQuestao validadorQuestao;

        public ServicoQuestao(IRepositorioQuestao repositorioQuestao, ValidadorQuestao validadorQuestao)
        {
            this.repositorioQuestao = repositorioQuestao;
            this.validadorQuestao = validadorQuestao;
        }

        public Result Inserir(Questao questao)
        {
            Log.Debug("Tentando inserir questão...{@q}", questao);

            List<string> erros = ValidarQuestao(questao);

            if (erros.Count() > 0)
                return Result.Fail(erros);

            try
            {
                repositorioQuestao.Inserir(questao);

                Log.Debug("Questão {QuestaoId} inserida com sucesso", questao.Id);

                return Result.Ok();
            }
            catch (Exception exc)
            {
                string msgErro = "Falha ao tentar inserir questão.";

                Log.Error(exc, msgErro + "{@q}", questao);

                return Result.Fail(msgErro);
            }
        }

        public Result Editar(Questao questao)
        {
            Log.Debug("Tentando editar questão...{@q}", questao);

            List<string> erros = ValidarQuestao(questao);

            if (erros.Count() > 0)
                return Result.Fail(erros);

            try
            {
                repositorioQuestao.Editar(questao);

                Log.Debug("Questão {QuestaoId} editada com sucesso", questao.Id);

                return Result.Ok();
            }
            catch (Exception exc)
            {
                string msgErro = "Falha ao tentar editar questão.";

                Log.Error(exc, msgErro + "{@q}", questao);

                return Result.Fail(msgErro);
            }
        }

        public Result Excluir(Questao questao)
        {
            Log.Debug("Tentando excluir questão...{@q}", questao);
            
            try
            {
                repositorioQuestao.Excluir(questao);

                Log.Debug("Questão {QuestaoId} excluída com sucesso", questao.Id);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                List<string> erros = new List<string>();

                string msgErro = ObterMensagemDeErro(ex);

                Log.Logger.Error(ex, msgErro + " {QuestaoId}", questao.Id);

                erros.Add(msgErro);

                return Result.Fail(erros);
            }
        }

        private string ObterMensagemDeErro(Exception ex)
        {
            string msgErro;

            if (ex.Message.Contains("FK_TBTesteTBQuestao_TBQuestao"))
                msgErro = "Esta questao está relacionada com um teste e não pode ser excluída";
            else
                msgErro = "Esta questão não pode ser excluída";

            return msgErro;
        }

        private List<string> ValidarQuestao(Questao questao)
        {
            List<string> erros = validadorQuestao.Validate(questao)
                .Errors.Select(x => x.ErrorMessage).ToList();

            return erros;
        }
    }
}