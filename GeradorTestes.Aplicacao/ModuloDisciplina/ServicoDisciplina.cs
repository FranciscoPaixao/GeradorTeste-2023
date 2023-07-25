using GeradorTestes.Dominio.ModuloDisciplina;

namespace GeradorTestes.Aplicacao.ModuloDisciplina
{
    public class ServicoDisciplina
    {
        private IRepositorioDisciplina repositorioDisciplina;
        private IValidadorDisciplina validadorDisciplina;

        public ServicoDisciplina(
            IRepositorioDisciplina repositorioDisciplina,
            IValidadorDisciplina validadorDisciplina)
        {
            this.repositorioDisciplina = repositorioDisciplina;
            this.validadorDisciplina = validadorDisciplina;
        }

        public Result Inserir(Disciplina disciplina)
        {
            Log.Debug("Tentando inserir disciplina...{@d}", disciplina);

            List<string> erros = ValidarDisciplina(disciplina);

            if (erros.Count() > 0)
                return Result.Fail(erros); //cenário 2

            try
            {
                repositorioDisciplina.Inserir(disciplina);

                Log.Debug("Disciplina {DisciplinaId} inserida com sucesso", disciplina.Id);

                return Result.Ok(); //cenário 1
            }
            catch (Exception exc)
            {
                string msgErro = "Falha ao tentar inserir disciplina.";

                Log.Error(exc, msgErro + "{@d}", disciplina);

                return Result.Fail(msgErro); //cenário 3
            }
        }
        
        public Result Editar(Disciplina disciplina)
        {
            Log.Debug("Tentando editar disciplina...{@d}", disciplina);

            List<string> erros = ValidarDisciplina(disciplina);

            if (erros.Count() > 0)
                return Result.Fail(erros);

            try
            {
                repositorioDisciplina.Editar(disciplina);

                Log.Debug("Disciplina {DisciplinaId} editada com sucesso", disciplina.Id);

                return Result.Ok();
            }
            catch (Exception exc)
            {
                string msgErro = "Falha ao tentar editar disciplina.";

                Log.Error(exc, msgErro + "{@d}", disciplina);

                return Result.Fail(msgErro);
            }
        }

        public Result Excluir(Disciplina disciplina)
        {
            Log.Debug("Tentando excluir disciplina...{@d}", disciplina);

            try
            {
                bool disciplinaExiste = repositorioDisciplina.Existe(disciplina);

                if (disciplinaExiste == false)
                {
                    Log.Warning("Disciplina {DisciplinaId} não encontrada para excluir", disciplina.Id);

                    return Result.Fail("Disciplina não encontrada");
                }

                repositorioDisciplina.Excluir(disciplina);

                Log.Debug("Disciplina {DisciplinaId} excluída com sucesso", disciplina.Id);

                return Result.Ok();
            }
            catch (SqlException ex)
            {
                List<string> erros = new List<string>();
              
                string msgErro;

                if (ex.Message.Contains("FK_TBMateria_TBDisciplina"))
                    msgErro = "Esta disciplina está relacionada com uma matéria e não pode ser excluída";
                else
                    msgErro = "Falha ao tentar excluir disciplina";

                erros.Add(msgErro);

                Log.Error(ex, msgErro + " {DisciplinaId}", disciplina.Id);

                return Result.Fail(erros);
            }
        }

        private List<string> ValidarDisciplina(Disciplina disciplina)
        {
            var resultadoValidacao = validadorDisciplina.Validate(disciplina);

            List<string> erros = new List<string>();

            if (resultadoValidacao != null)
                erros.AddRange(resultadoValidacao.Errors.Select(x => x.ErrorMessage));

            if (NomeDuplicado(disciplina))
                erros.Add($"Este nome '{disciplina.Nome}' já está sendo utilizado");

            foreach (string erro in erros)
            {
                Log.Warning(erro);
            }

            return erros;
        }

        private bool NomeDuplicado(Disciplina disciplina)
        {
            Disciplina disciplinaEncontrada = repositorioDisciplina.SelecionarPorNome(disciplina.Nome);

            if (disciplinaEncontrada != null &&
                disciplinaEncontrada.Id != disciplina.Id &&
                disciplinaEncontrada.Nome == disciplina.Nome)
            {
                return true;
            }

            return false;
        }

    }
}
