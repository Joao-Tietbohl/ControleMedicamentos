using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {

        RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        RepositorioFornecedorEmBancoDeDados repositorioFornecedor;

        Medicamento m;
        Fornecedor f;

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();

            f = new Fornecedor("Joao", "(54) 9907-0988", "joaodsnjkcna@gmail.com", "Bonja", "RS");
            m = new Medicamento("methamphetamine", "Science", "a-25", new DateTime(2022, 10, 30), f);

            DB.ExecuteSQL("DELETE FROM [TBMEDICAMENTO]; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            

        }

        [TestMethod]
        public void Deve_inserir_medicamento()
        {

            repositorioFornecedor.Inserir(f);
            repositorioMedicamento.Inserir(m);
   
            List<Medicamento> l = repositorioMedicamento.SelecionarTodos();

            //Assert
            Assert.AreEqual(m.Nome, l[0].Nome);
        }

        [TestMethod]
        public void Deve_Editar_Medicamento()
        {
            repositorioFornecedor.Inserir(f);
            repositorioMedicamento.Inserir(m);

            m.Nome = "Astro";
            m.Lote = "f-11";
            m.QuantidadeDisponivel = 5;
            m.Descricao = "Antibiotico";
            m.Fornecedor = f;

            repositorioMedicamento.Editar(m);

            List<Medicamento> l = repositorioMedicamento.SelecionarTodos();

            Assert.AreEqual("Astro", l[0].Nome);
        }

        [TestMethod]
        public void Deve_Excluir_Medicamento()
        {
            repositorioFornecedor.Inserir(f);
            repositorioMedicamento.Inserir(m);

            repositorioMedicamento.Excluir(m);

            List<Medicamento> l = repositorioMedicamento.SelecionarTodos();

            Assert.AreEqual(0, l.Count);
        }
    }
}
