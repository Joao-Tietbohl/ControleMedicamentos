using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDeDados
    {
        string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;
                       Initial Catalog=ControleMedicamentos;Integrated Security=True";

        public ValidationResult Inserir(Fornecedor novoFornecedor)
        {
            var validator = ObterValidador();

            var resultadoValidacaoFornecedor = validator.Validate(novoFornecedor);


            if (resultadoValidacaoFornecedor.IsValid == false)
                return resultadoValidacaoFornecedor;

            string sqlInsercao =
                @"INSERT INTO [dbo].[TBFornecedor]
                        ([Nome]
                        ,[Telefone]
                        ,[Email]
                        ,[Cidade]
                        ,[Estado])
                  VALUES
                        (@Nome, 
	               		@Telefone,
	               		@Email,
	               		@Cidade, 
	               		@Estado
	               	   )
	               	   SELECT SCOPE_IDENTITY()";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInserir = new SqlCommand(sqlInsercao, conexaoComBanco);

            ConfigurarParametrosFornecedor(novoFornecedor, comandoInserir);


            conexaoComBanco.Open();

            var id = comandoInserir.ExecuteScalar();
            novoFornecedor.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacaoFornecedor;
        }
        public ValidationResult Editar(Fornecedor fornecedor)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(enderecoBanco, conexaoComBanco);

            string sql =
                @"UPDATE [DBO].[TBFORNECEDOR]
                   SET [NOME] = @NOME, 
                      [TELEFONE] = @TELEFONE,
                      [EMAIL] = @EMAIL,
                      [CIDADE] = @CIDADE,
                      [ESTADO] = @ESTADO
                 
                    WHERE
                      [ID] = @ID";

            comandoEdicao.CommandText = sql;

            var validator = ObterValidador();

            var resultadoValidacao = validator.Validate(fornecedor);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            ConfigurarParametrosFornecedor(fornecedor, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }
        public ValidationResult Excluir(Fornecedor fornecedor)
        {
            string sqlExcluir =
              @"DELETE FROM [TBFORNECEDOR]
		            WHERE 
		             [ID] = @ID
                     ";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", fornecedor.Id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o fornecedor"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }
        


        public List<Fornecedor> SelecionarTodos()
        {
            string sqlSelecionarTodos =
               @"SELECT [Id]
                      ,[Nome]
                      ,[Telefone]
                      ,[Email]
                      ,[Cidade]
                      ,[Estado]
                  FROM [dbo].[TBFornecedor]";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            List<Fornecedor> fornecedores = new List<Fornecedor>();

            while (leitorFornecedor.Read())
            {
                Fornecedor fornecedor = ConverterParaFornecedor(leitorFornecedor);

                fornecedores.Add(fornecedor);
            }

            conexaoComBanco.Close();

            return fornecedores;

        }

        private Fornecedor ConverterParaFornecedor(SqlDataReader leitorFornecedor)
        {
            var id = Convert.ToInt32(leitorFornecedor["ID"]);
            var nome = Convert.ToString(leitorFornecedor["NOME"]);
            var telefone = Convert.ToString(leitorFornecedor["TELEFONE"]);
            var email = Convert.ToString(leitorFornecedor["EMAIL"]);
            var cidade = Convert.ToString(leitorFornecedor["CIDADE"]);
            var estado = Convert.ToString(leitorFornecedor["ESTADO"]);


            var fornecedor = new Fornecedor
            {
                Id = id,
                Nome = nome,
                Telefone = telefone,
                Email = email,
                Cidade = cidade,
                Estado = estado

            };

            return fornecedor;
        }

        private AbstractValidator<Fornecedor> ObterValidador()
        {
          return new ValidadorFornecedor();
        }

        private void ConfigurarParametrosFornecedor(Fornecedor fornecedor, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", fornecedor.Id);
            comando.Parameters.AddWithValue("NOME", fornecedor.Nome);
            comando.Parameters.AddWithValue("TELEFONE", fornecedor.Telefone);
            comando.Parameters.AddWithValue("EMAIL", fornecedor.Email);
            comando.Parameters.AddWithValue("CIDADE", fornecedor.Cidade);
            comando.Parameters.AddWithValue("ESTADO", fornecedor.Estado);


        }
    }
}
