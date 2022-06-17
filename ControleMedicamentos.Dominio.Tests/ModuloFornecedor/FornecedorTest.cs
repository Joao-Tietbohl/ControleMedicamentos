using ControleMedicamentos.Dominio.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.Tests.ModuloFornecedor
{
    [TestClass]
    public class FornecedorTest
    {
        ValidadorFornecedor validador;

        public FornecedorTest()
        {
            validador = new ValidadorFornecedor();
        }

        [TestMethod]
        public void Nome_Deve_Ser_Valido()
        { 
            //Arrange
            Fornecedor f = new Fornecedor();
            f.Nome = null;

            //Action
            var resultado = validador.Validate(f);

            //Assert
            Assert.AreEqual("Nome inválido!", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Estado_Deve_Ser_Valido()
        {
            //Arrange
            Fornecedor f = new Fornecedor();
            f.Nome = "JoaoDosVeneno";
            f.Estado = "";
            //Action
            var resultado = validador.Validate(f);

            //Assert
            Assert.AreEqual("Estado inválido!", resultado.Errors[0].ErrorMessage);
        }


        [TestMethod]
        public void Cidade_Deve_Ser_Valida()
        {
            //Arrange
            Fornecedor f = new Fornecedor();
            f.Nome = "JoaoDosVeneno";
            f.Estado = "SC";
            f.Cidade = null;

            //Action
            var resultado = validador.Validate(f);

            //Assert
            Assert.AreEqual("Cidade inválida!", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Telefone_Deve_Ser_Valido()
        {
            //Arrange
            Fornecedor f = new Fornecedor();
            f.Nome = "JoaoDosVeneno";
            f.Estado = "SC";
            f.Cidade = "Joinville";
            f.Telefone = "549907-0988";


            //Action
            var resultado = validador.Validate(f);

            //Assert
            Assert.AreEqual("'Telefone' is not in the correct format.", resultado.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Email_Deve_Ser_Valido()
        {
            //Arrange
            Fornecedor f = new Fornecedor();
            f.Nome = "JoaoDosVeneno";
            f.Estado = "SC";
            f.Cidade = "Joinville";
            f.Telefone = "(54) 9907-0988";
            f.Email = "zvcx";


            //Action
            var resultado = validador.Validate(f);

            //Assert
            Assert.AreEqual("Email inválido!", resultado.Errors[0].ErrorMessage);
        }
    }
}
