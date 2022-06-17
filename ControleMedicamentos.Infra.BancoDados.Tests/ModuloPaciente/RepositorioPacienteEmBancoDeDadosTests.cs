using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDeDadosTests
    {
        RepositorioPacienteEmBancoDeDados repositorio;
        Paciente p;

        public RepositorioPacienteEmBancoDeDadosTests()
        {
            p = new Paciente("Radahn", "123151568");
            repositorio = new RepositorioPacienteEmBancoDeDados();

            DB.ExecuteSQL("DELETE FROM[TBPACIENTE]; DBCC CHECKIDENT(TBFORNECEDOR, RESEED, 0)");
        }

        [TestMethod]
        public void Deve_Inserir_Paciente()
        {
            repositorio.Inserir(p);

            List<Paciente> l = repositorio.SelecionarTodos();

            Assert.AreEqual(p, l[0]);
        }

        [TestMethod]
        public void Deve_Editar_Paciente()
        {

            repositorio.Inserir(p);

            p.Nome = "Alexandre";
            p.CartaoSUS = "1156";

            repositorio.Editar(p);

            List<Paciente> l = repositorio.SelecionarTodos();

            Assert.AreEqual(p, l[0]);
        }

        [TestMethod]
        public void Deve_Excluir_Paciente()
        {
            repositorio.Inserir(p);

            repositorio.Excluir(p);

            List<Paciente> l = repositorio.SelecionarTodos();

            Assert.AreEqual(0, l.Count);
        }
    }
}
