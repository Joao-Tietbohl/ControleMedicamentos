using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.RepositorioFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDeDadosTests
    {
        RepositorioFuncionarioEmBancoDeDados repositorio;
        Funcionario f;
        public RepositorioFuncionarioEmBancoDeDadosTests()
        {
            repositorio = new RepositorioFuncionarioEmBancoDeDados();
            f = new Funcionario("Pedro", "Pedro123", "Pedro123");
            DB.ExecuteSQL("DELETE FROM [TBFUNCIONARIO]; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
        }

        [TestMethod]
        public void Deve_Inserir_Funcionario()
        {
            repositorio.Inserir(f);

            List<Funcionario> l = repositorio.SelecionarTodos();

            Assert.AreEqual(f, l[0]);
        }

        [TestMethod]
        public void Deve_Editar_Funcionario()
        {

            repositorio.Inserir(f);

            f.Nome = "Alexandre";
            f.Senha = "1156";
            f.Login = "dsadsa";

            repositorio.Editar(f);

            List<Funcionario> l = repositorio.SelecionarTodos();

            Assert.AreEqual(f, l[0]);
        }

        [TestMethod]
        public void Deve_Excluir_Paciente()
        {
            repositorio.Inserir(f);

            repositorio.Excluir(f);

            List<Funcionario> l = repositorio.SelecionarTodos();

            Assert.AreEqual(0, l.Count);
        }
    }
}
