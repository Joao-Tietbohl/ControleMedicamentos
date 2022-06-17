using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDeDados
    {
        string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog=ControleMedicamentos;Integrated Security=True";

        public ValidationResult Inserir(Paciente novoPaciente)
        {
            var validator = ObterValidador();

            var resultadoValidacaoPaciente = validator.Validate(novoPaciente);


            if (resultadoValidacaoPaciente.IsValid == false)
                return resultadoValidacaoPaciente;

            string sqlInsercao =
                @"INSERT INTO [TBPACIENTE]
                      (
               	       [NOME],       
                       [CARTAOSUS]
            		   )
                VALUES
                      (
            		   @NOME,
            		   @CARTAOSUS
            		   );
            		   SELECT SCOPE_IDENTITY();";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInserir = new SqlCommand(sqlInsercao, conexaoComBanco);

            ConfigurarParametrosPaciente(novoPaciente, comandoInserir);


            conexaoComBanco.Open();

            var id = comandoInserir.ExecuteScalar();
            novoPaciente.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacaoPaciente;
        }
        public ValidationResult Editar(Paciente paciente)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(enderecoBanco, conexaoComBanco);

            string sql =
                @"UPDATE [DBO].[TBPACIENTE]
                   SET [NOME] = @NOME, 
                      [CARTAOSUS] = @CARTAOSUS
                 
                    WHERE
                      [ID] = @ID";

            comandoEdicao.CommandText = sql;

            var validator = ObterValidador();

            var resultadoValidacao = validator.Validate(paciente);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            ConfigurarParametrosPaciente(paciente, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }
        public ValidationResult Excluir(Paciente paciente)
        {
            string sqlExcluir =
                          @"DELETE FROM [TBPACIENTE]
		            WHERE 
		             [ID] = @ID
                     ";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", paciente.Id);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o paciente"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Paciente> SelecionarTodos()
        {
            string sqlSelecionarTodos =
               @"SELECT [Id]
                      ,[Nome]
                      ,[CartaoSUS]
                     
                  FROM [TBPaciente]";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (leitorPaciente.Read())
            {
                Paciente paciente = ConverterParaPaciente(leitorPaciente);

                pacientes.Add(paciente);
            }

            conexaoComBanco.Close();

            return pacientes;

        }

        private Paciente ConverterParaPaciente(SqlDataReader leitorPaciente)
        {
            var id = Convert.ToInt32(leitorPaciente["ID"]);
            var nome = Convert.ToString(leitorPaciente["NOME"]);
            var cartaoSUS = Convert.ToString(leitorPaciente["CARTAOSUS"]);

            var paciente = new Paciente
            {
                Id = id,
                Nome = nome,
                CartaoSUS = cartaoSUS,

            };

            return paciente;
        }

        private void ConfigurarParametrosPaciente(Paciente paciente, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", paciente.Id);
            comando.Parameters.AddWithValue("NOME", paciente.Nome);
            comando.Parameters.AddWithValue("CARTAOSUS", paciente.CartaoSUS); 
        }

        private AbstractValidator<Paciente> ObterValidador()
        {
            return new ValidadorPaciente();
        }
    }
}
