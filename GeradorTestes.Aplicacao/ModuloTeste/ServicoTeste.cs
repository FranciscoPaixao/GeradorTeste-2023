using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;

namespace GeradorTestes.Aplicacao.ModuloTeste
{
    public class ServicoTeste
    {
        private IRepositorioQuestao repositorioQuestao;
        private IRepositorioTeste repositorioTeste;
        private ValidadorTeste validadorTeste;
        private IGeradorArquivo geradorArquivoPdf;

        public ServicoTeste(IRepositorioTeste repositorioTeste, IRepositorioQuestao repositorioQuestao, ValidadorTeste validadorTeste, IGeradorArquivo geradorArquivoPdf)
        {
            this.repositorioTeste = repositorioTeste;
            this.repositorioQuestao = repositorioQuestao;
            this.validadorTeste = validadorTeste;
            this.geradorArquivoPdf = geradorArquivoPdf;
        }

        public Result Inserir(Teste teste)
        {
            Log.Debug("Tentando inserir teste...{@t}", teste);

            List<string> erros = ValidarTeste(teste);

            if (erros.Count() > 0)
                return Result.Fail(erros);

            try
            {
                repositorioTeste.Inserir(teste);

                foreach (Questao questao in teste.Questoes)
                {
                    questao.JaUtilizada = true;
                    repositorioQuestao.Editar(questao);
                }

                Log.Debug("Teste {TesteId} inserido com sucesso", teste.Id);

                return Result.Ok();
            }
            catch (Exception exc)
            {
                string msgErro = "Falha ao tentar inserir teste.";

                Log.Error(exc, msgErro + "{@t}", teste);

                return Result.Fail(msgErro);
            }
        }

        public Result Excluir(Teste teste)
        {
            Log.Debug("Tentando excluir teste...{TesteId}", teste.Id);

            try
            {
                repositorioTeste.Excluir(teste);

                foreach (Questao questao in teste.Questoes)
                {
                    questao.JaUtilizada = false;
                    repositorioQuestao.Editar(questao);
                }

                Log.Debug("Teste {TesteId} excluído com sucesso", teste.Id);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                List<string> erros = new List<string>();

                string msgErro = "Falha ao tentar exclluir teste selecionado...";

                Log.Logger.Error(ex, msgErro + " {TesteId}", teste.Id);

                erros.Add(msgErro);

                return Result.Fail(erros);
            }
        }

        public Result GerarTesteEmPdf(Teste teste, string diretorio, bool gerarGabarito)
        {
            Log.Debug("Tentando gerar PDF do teste...{TesteId}", teste.Id);

            try
            {
                geradorArquivoPdf.GerarTeste(teste, diretorio);

                Log.Debug("Teste {TesteId} em PDF gerado com sucesso", teste.Id);

                if (gerarGabarito)
                {
                    geradorArquivoPdf.GerarGabarito(teste, diretorio);

                    Log.Debug("Gabarito do Teste {TesteId} em PDF gerado com sucesso", teste.Id);
                }                                

                return Result.Ok();
            }
            catch (Exception ex)
            {
                List<string> erros = new List<string>();

                string msgErro = "Falha ao tentar gerar PDF do teste selecionado.";

                Log.Logger.Error(ex, msgErro + " {TesteId}", teste.Id);

                erros.Add(msgErro);

                return Result.Fail(erros);
            }
        }

        private List<string> ValidarTeste(Teste teste)
        {
            List<string> erros = validadorTeste.Validate(teste)
               .Errors.Select(x => x.ErrorMessage).ToList();

            return erros;
        }

        
    }
}