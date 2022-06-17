using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=ControleMedicamentos;Integrated Security=True";

        public ValidationResult Inserir(Medicamento novoMedicamento)
        {
            var validator = ObterValidador();

            var resultadoValidacaoMedicamento = validator.Validate(novoMedicamento);


            if (resultadoValidacaoMedicamento.IsValid == false)
                return resultadoValidacaoMedicamento;

            string sqlInsercao =
                @"INSERT INTO [TBMEDICAMENTO]
           (
    	    [NOME]       
           ,[DESCRICAO]
           ,[LOTE]
           ,[VALIDADE]
           ,[QUANTIDADEDISPONIVEL]
           ,[FORNECEDOR_ID]
		   )
     VALUES
           (
		   @NOME,
		   @DESCRICAO,
           @LOTE,
		   @VALIDADE, 
           @QUANTIDADEDISPONIVEL,
           @FORNECEDOR_ID
		   );
		   SELECT SCOPE_IDENTITY();";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInserir = new SqlCommand(sqlInsercao, conexaoComBanco);

            ConfigurarParametrosMedicamento(novoMedicamento, comandoInserir);


            conexaoComBanco.Open();

            var id = comandoInserir.ExecuteScalar();
            novoMedicamento.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacaoMedicamento;
        }
        public ValidationResult Editar(Medicamento medicamento)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(enderecoBanco, conexaoComBanco);

            string sql =
                @"UPDATE   [TBMEDICAMENTO]
	            SET
	            	
	            	[NOME] = @NOME,
	            	[DESCRICAO] = @DESCRICAO,
	            	[LOTE] = @LOTE,
                    [VALIDADE] = @VALIDADE,
                    [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,
                    [FORNECEDOR_ID] = @FORNECEDOR_ID 
	            	
	            WHERE 
	            	[ID] = @ID";

            comandoEdicao.CommandText = sql;

            var validator = ObterValidador();

            var resultadoValidacao = validator.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            ConfigurarParametrosMedicamento(medicamento, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }
        public ValidationResult Excluir(Medicamento medicamento)
        {
            string sqlExcluir =
               @"DELETE FROM [TBMEDICAMENTO]
		            WHERE 
		             [ID] = @ID
                     ";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", medicamento.Id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o medicamento"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }



        public List<Medicamento> SelecionarTodos()
        {
            string sqlSelecionarTodos =
               @"SELECT 
                	M.ID,
                	M.NOME,
                	M.DESCRICAO,
                	M.LOTE,
                    M.VALIDADE,
                    M.QUANTIDADEDISPONIVEL ,
                    F.ID AS FORNECEDOR_ID
                FROM	
                	TBMEDICAMENTO AS M INNER JOIN 
                	TBFORNECEDOR AS F
                ON 
                	M.FORNECEDOR_ID = F.ID";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            List<Medicamento> materias = new List<Medicamento>();

            while (leitorMedicamento.Read())
            {
                Medicamento medicamento = ConverterParaMedicamento(leitorMedicamento);

                materias.Add(medicamento);
            }

            conexaoComBanco.Close();

            return materias;
        }

        private AbstractValidator<Medicamento> ObterValidador()
        {
            return new ValidadorMedicamento();
        }

        private void ConfigurarParametrosMedicamento(Medicamento medicamento, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", medicamento.Id);
            comando.Parameters.AddWithValue("NOME", medicamento.Nome);
            comando.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            comando.Parameters.AddWithValue("LOTE", medicamento.Lote);
            comando.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            comando.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);
            comando.Parameters.AddWithValue("FORNECEDOR_ID", medicamento.Fornecedor.Id);


        }

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {
            var id = Convert.ToInt32(leitorMedicamento["ID"]);
            var nome = Convert.ToString(leitorMedicamento["NOME"]);
            var descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            var lote = Convert.ToString(leitorMedicamento["LOTE"]);
            var validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);
            var quantidadeDisponivel = Convert.ToInt32(leitorMedicamento["QUANTIDADEDISPONIVEL"]);
            var fornecedor_ID = Convert.ToInt32(leitorMedicamento["FORNECEDOR_ID"]);


            var medicamento = new Medicamento
            {
                Id = id,
                Nome = nome,
                Descricao = descricao,
                Lote = lote,
                Validade = validade,
                QuantidadeDisponivel = quantidadeDisponivel,
                Fornecedor = new Fornecedor
                {
                    Id = fornecedor_ID,
                }

            };

            return medicamento;
        }
    }
}
