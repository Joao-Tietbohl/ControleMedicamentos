using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Compartilhado
{
    public static class DB
    {
       static string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;
                       Initial Catalog=ControleMedicamentos;Integrated Security=True";

        public static void ExecuteSQL(string sql)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }
    }
}
