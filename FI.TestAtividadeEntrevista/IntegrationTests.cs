using Microsoft.VisualStudio.TestTools.UnitTesting;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;

namespace FI.TestAtividadeEntrevista
{
    [TestClass]
    public class IntegrationTests
    {
        /// <summary>
        /// Teste de integração que valida o fluxo completo de Cliente e Beneficiário
        /// </summary>
        [TestMethod]
        public void FluxoCompleto_ClienteComBeneficiarios_DeveExecutarComSucesso()
        {
            // Arrange
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            var cliente = new Cliente
            {
                Nome = "José",
                Sobrenome = "Silva",
                CPF = "10203040506",
                Email = "jose.silva@email.com",
                CEP = "12345-678",
                Cidade = "São Paulo",
                Estado = "SP",
                Logradouro = "Rua Integração, 100",
                Nacionalidade = "Brasileiro",
                Telefone = "(11) 99999-0000"
            };

            // Act - Incluir Cliente
            long idCliente = boCliente.Incluir(cliente);
            Assert.IsTrue(idCliente > 0, "Cliente deve ser incluído com sucesso");

            // Act - Incluir Beneficiários
            var beneficiario1 = new Beneficiario
            {
                IdCliente = idCliente,
                CPF = "11111111111",
                Nome = "Maria Silva"
            };
            var beneficiario2 = new Beneficiario
            {
                IdCliente = idCliente,
                CPF = "22222222222",
                Nome = "João Silva"
            };

            long idBeneficiario1 = boBeneficiario.Incluir(beneficiario1);
            long idBeneficiario2 = boBeneficiario.Incluir(beneficiario2);

            Assert.IsTrue(idBeneficiario1 > 0, "Beneficiário 1 deve ser incluído");
            Assert.IsTrue(idBeneficiario2 > 0, "Beneficiário 2 deve ser incluído");

            // Act - Consultar Cliente com Beneficiários
            var clienteConsultado = boCliente.Consultar(idCliente);

            // Assert
            Assert.IsNotNull(clienteConsultado, "Cliente deve ser encontrado");
            Assert.AreEqual(idCliente, clienteConsultado.Id);
            Assert.AreEqual("José", clienteConsultado.Nome);
            Assert.IsNotNull(clienteConsultado.Beneficiarios, "Beneficiários não devem ser nulos");
            Assert.AreEqual(2, clienteConsultado.Beneficiarios.Count, "Deve haver 2 beneficiários");

            // Cleanup
            boBeneficiario.Excluir(idBeneficiario1);
            boBeneficiario.Excluir(idBeneficiario2);
            boCliente.Excluir(idCliente);
        }
    }
}
