using ControleMedicamentos.Dominio.ModuloMedicamento;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
        ValidadorMedicamento validador;

        public MedicamentoTest()
        {
            validador = new ValidadorMedicamento();
        }

        [TestMethod]
        public void Nome_Deve_Ser_Valido()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "";

            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Nome inválido!", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Descricao_Deve_Ser_Valida()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "Astro";
            m.Descricao = null;
            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Descrição inválida!", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Lote_Deve_Ser_Valido()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "Astro";
            m.Descricao = "Antibiotico";
            m.Lote = "";

            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Lote inválido!", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Quantidade_Deve_Ser_Valido()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "Astro";
            m.Descricao = "Antibiotico";
            m.Lote = "1315";
            m.QuantidadeDisponivel = -1;

            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Quantidade de medicamento inválida!", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Validade_Deve_Ser_Valida()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "Astro";
            m.Descricao = "Antibiotico";
            m.Lote = "1315";
            m.QuantidadeDisponivel = 10;
            m.Validade = new System.DateTime(2000,5,5);
            

            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Validade inválida!", resultado.Errors[0].ErrorMessage);

        }

        [TestMethod]
        public void Fornecedor_Deve_Ser_Valido()
        {
            //Arrange
            Medicamento m = new Medicamento();
            m.Nome = "Astro";
            m.Descricao = "Antibiotico";
            m.Lote = "1315";
            m.QuantidadeDisponivel = 10;
            m.Validade = new System.DateTime(2022,10,10);
            m.Fornecedor = null;

            //Action
            var resultado = validador.Validate(m);

            //Assert
            Assert.AreEqual("Fornecedor inválido!", resultado.Errors[0].ErrorMessage);

        }
    }
}
