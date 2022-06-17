using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDeDados
    {

        string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;
                       Initial Catalog=ControleMedicamentos;Integrated Security=True";

        public ValidationResult Inserir(Funcionario novoFuncionario)
        {
            var validator = ObterValidador();

            var resultadoValidacaoFuncionario = validator.Validate(novoFuncionario);


            if (resultadoValidacaoFuncionario.IsValid == false)
                return resultadoValidacaoFuncionario;

            string sqlInsercao =
                @"INSERT INTO [dbo].[TBFuncionario]
                        ([Nome]
                        ,[Login]
                        ,[Senha]
                        )
                  VALUES
                        (@Nome, 
	               		@Login,
	               		@Senha
	               		
	               	   )
	               	   SELECT SCOPE_IDENTITY()";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInserir = new SqlCommand(sqlInsercao, conexaoComBanco);

            ConfigurarParametrosFuncionario(novoFuncionario, comandoInserir);


            conexaoComBanco.Open();

            var id = comandoInserir.ExecuteScalar();
            novoFuncionario.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacaoFuncionario;
        }
        public ValidationResult Editar(Funcionario funcionario)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(enderecoBanco, conexaoComBanco);

            string sql =
                @"UPDATE [DBO].[TBFUNCIONARIO]
                   SET [NOME] = @NOME, 
                      [LOGIN] = @LOGIN,
                      [SENHA] = @SENHA
                 
                    WHERE
                      [ID] = @ID";

            comandoEdicao.CommandText = sql;

            var validator = ObterValidador();

            var resultadoValidacao = validator.Validate(funcionario);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            ConfigurarParametrosFuncionario(funcionario, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }
        public ValidationResult Excluir(Funcionario funcionario)
        {
            string sqlExcluir =
             @"DELETE FROM [TBFUNCIONARIO]
		            WHERE 
		             [ID] = @ID
                     ";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", funcionario.Id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o funcionario"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }


        public List<Funcionario> SelecionarTodos()
        {
            string sqlSelecionarTodos =
               @"SELECT [ID]
                      ,[NOME]
                      ,[LOGIN]
                      ,[SENHA]
                     
                  FROM [DBO].[TBFUNCIONARIO]";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            List<Funcionario> fornecedores = new List<Funcionario>();

            while (leitorFuncionario.Read())
            {
                Funcionario funcionario = ConverterParaFornecedor(leitorFuncionario);

                fornecedores.Add(funcionario);
            }

            conexaoComBanco.Close();

            return fornecedores;

        }
        private Funcionario ConverterParaFornecedor(SqlDataReader leitorFuncionario)
        {
            var id = Convert.ToInt32(leitorFuncionario["ID"]);
            var nome = Convert.ToString(leitorFuncionario["NOME"]);
            var login = Convert.ToString(leitorFuncionario["LOGIN"]);
            var senha = Convert.ToString(leitorFuncionario["SENHA"]);
            


            var funcionario = new Funcionario
            {
                Id = id,
                Nome = nome,
                Login = login,
                Senha = senha,
               

            };

            return funcionario;
        }
        private AbstractValidator<Funcionario> ObterValidador()
        {
            return new ValidadorFuncionario();
        }
        private void ConfigurarParametrosFuncionario(Funcionario funcionario, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", funcionario.Id);
            comando.Parameters.AddWithValue("NOME", funcionario.Nome);
            comando.Parameters.AddWithValue("LOGIN", funcionario.Login);
            comando.Parameters.AddWithValue("SENHA", funcionario.Senha);
           


        }
    }
}
