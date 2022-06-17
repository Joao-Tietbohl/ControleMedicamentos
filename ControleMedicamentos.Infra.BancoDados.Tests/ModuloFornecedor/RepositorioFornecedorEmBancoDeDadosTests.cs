using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDeDadosTests
    {

        RepositorioFornecedorEmBancoDeDados repositorio;
        Fornecedor f;

        public RepositorioFornecedorEmBancoDeDadosTests()
        {
            repositorio = new RepositorioFornecedorEmBancoDeDados();
            f = new Fornecedor("Joao", "(54) 9907-0988", "joaodsnjkcna@gmail.com", "Bonja", "RS");
            
            DB.ExecuteSQL("DELETE FROM [TBMEDICAMENTO]; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            DB.ExecuteSQL("DELETE FROM [TBFORNECEDOR]; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
        }

        [TestMethod]
        public void Deve_Inserir_Fornecedor()
        {
          
            repositorio.Inserir(f);
            List<Fornecedor> l = repositorio.SelecionarTodos();

            Assert.AreEqual(f, l[0]);
        }

        [TestMethod]
        public void Deve_Editar_Fornecedor()
        {
            repositorio.Inserir(f);

            f.Nome = "Pfizer";
            f.Email = "email@gmail.com";
            f.Telefone = "(54) 9908-0977";
            f.Cidade = "Lages";
            f.Estado = "SC";

            repositorio.Editar(f);

            List<Fornecedor> l = repositorio.SelecionarTodos();
            Assert.AreEqual(f, l[0]);
        }

        [TestMethod]
        public void Deve_Excluir_Fornecedor()
        {
            repositorio.Inserir(f);

            repositorio.Excluir(f);

            List<Fornecedor> l = repositorio.SelecionarTodos();

            Assert.AreEqual(0, l.Count);
        }
    }
}
