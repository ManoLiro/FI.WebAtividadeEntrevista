using Microsoft.VisualStudio.TestTools.UnitTesting;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System.Linq;

namespace FI.TestAtividadeEntrevista
{
    [TestClass]
    public class BoClienteTests
    {
        private BoCliente _boCliente;

        [TestInitialize]
        public void Setup()
        {
            _boCliente = new BoCliente();
        }

        [TestMethod]
        public void Incluir_DeveRetornarIdValido_QuandoClienteValido()
        {
            var cliente = new Cliente
            {
                Nome = "João",
                Sobrenome = "Silva",
                CPF = "12345678901",
                Email = "joao.silva@email.com",
                CEP = "12345-678",
                Cidade = "São Paulo",
                Estado = "SP",
                Logradouro = "Rua Teste, 123",
                Nacionalidade = "Brasileiro",
                Telefone = "(11) 98765-4321"
            };

            long id = _boCliente.Incluir(cliente);

            Assert.IsTrue(id > 0, "O ID retornado deve ser maior que zero");
            _boCliente.Excluir(id);
        }

        [TestMethod]
        public void Consultar_DeveRetornarCliente_QuandoIdExiste()
        {
            var clienteOriginal = new Cliente
            {
                Nome = "Maria",
                Sobrenome = "Santos",
                CPF = "98765432109",
                Email = "maria.santos@email.com",
                CEP = "98765-432",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Logradouro = "Av. Principal, 456",
                Nacionalidade = "Brasileira",
                Telefone = "(21) 91234-5678"
            };
            long id = _boCliente.Incluir(clienteOriginal);
           
            var clienteConsultado = _boCliente.Consultar(id);

            Assert.IsNotNull(clienteConsultado, "Cliente não deve ser nulo");
            Assert.AreEqual(id, clienteConsultado.Id);
            Assert.AreEqual(clienteOriginal.Nome, clienteConsultado.Nome);
            Assert.AreEqual(clienteOriginal.Sobrenome, clienteConsultado.Sobrenome);
            Assert.AreEqual(clienteOriginal.CPF, clienteConsultado.CPF);
            Assert.AreEqual(clienteOriginal.Email, clienteConsultado.Email);

            _boCliente.Excluir(id);
        }

        [TestMethod]
        public void Consultar_DeveRetornarNull_QuandoIdNaoExiste()
        {
            long idInexistente = 999999999;

            var cliente = _boCliente.Consultar(idInexistente);

            Assert.IsNull(cliente, "Cliente deve ser nulo quando ID não existe");
        }

        [TestMethod]
        public void Alterar_DeveAtualizarCliente_QuandoDadosValidos()
        {
            var cliente = new Cliente
            {
                Nome = "Pedro",
                Sobrenome = "Oliveira",
                CPF = "11122233344",
                Email = "pedro.oliveira@email.com",
                CEP = "11111-222",
                Cidade = "Belo Horizonte",
                Estado = "MG",
                Logradouro = "Rua Nova, 789",
                Nacionalidade = "Brasileiro",
                Telefone = "(31) 99999-8888"
            };
            long id = _boCliente.Incluir(cliente);

            cliente.Id = id;
            cliente.Nome = "Pedro Henrique";
            cliente.Email = "pedro.henrique@email.com";
            _boCliente.Alterar(cliente);

            var clienteAlterado = _boCliente.Consultar(id);

            Assert.IsNotNull(clienteAlterado);
            Assert.AreEqual("Pedro Henrique", clienteAlterado.Nome);
            Assert.AreEqual("pedro.henrique@email.com", clienteAlterado.Email);

            
            _boCliente.Excluir(id);
        }

        [TestMethod]
        public void Excluir_DeveRemoverCliente_QuandoIdExiste()
        {
            var cliente = new Cliente
            {
                Nome = "Ana",
                Sobrenome = "Costa",
                CPF = "55566677788",
                Email = "ana.costa@email.com",
                CEP = "22222-333",
                Cidade = "Curitiba",
                Estado = "PR",
                Logradouro = "Rua Exclusão, 321",
                Nacionalidade = "Brasileira",
                Telefone = "(41) 98888-7777"
            };
            long id = _boCliente.Incluir(cliente);

            _boCliente.Excluir(id);
            var clienteExcluido = _boCliente.Consultar(id);

            Assert.IsNull(clienteExcluido, "Cliente deve ser nulo após exclusão");
        }

        [TestMethod]
        public void Listar_DeveRetornarListaDeClientes()
        {
            var cliente1 = new Cliente
            {
                Nome = "Carlos",
                Sobrenome = "Lima",
                CPF = "33344455566",
                Email = "carlos.lima@email.com",
                CEP = "33333-444",
                Cidade = "Porto Alegre",
                Estado = "RS",
                Logradouro = "Rua Lista, 111",
                Nacionalidade = "Brasileiro",
                Telefone = "(51) 97777-6666"
            };
            var cliente2 = new Cliente
            {
                Nome = "Beatriz",
                Sobrenome = "Ferreira",
                CPF = "77788899900",
                Email = "beatriz.ferreira@email.com",
                CEP = "44444-555",
                Cidade = "Salvador",
                Estado = "BA",
                Logradouro = "Av. Listar, 222",
                Nacionalidade = "Brasileira",
                Telefone = "(71) 96666-5555"
            };

            long id1 = _boCliente.Incluir(cliente1);
            long id2 = _boCliente.Incluir(cliente2);

            var clientes = _boCliente.Listar();

            Assert.IsNotNull(clientes);
            Assert.IsTrue(clientes.Count >= 2, "Deve haver pelo menos 2 clientes na lista");
            Assert.IsTrue(clientes.Any(c => c.Id == id1));
            Assert.IsTrue(clientes.Any(c => c.Id == id2));
           
            _boCliente.Excluir(id1);
            _boCliente.Excluir(id2);
        }

        [TestMethod]
        public void Pesquisa_DeveRetornarClientesPaginados()
        {        
            int qtd;
 
            var clientes = _boCliente.Pesquisa(0, 10, "Nome", true, out qtd);
            
            Assert.IsNotNull(clientes);
            Assert.IsTrue(clientes.Count <= 10, "Deve retornar no máximo 10 clientes");
            Assert.IsTrue(qtd >= 0, "Quantidade total deve ser maior ou igual a zero");
        }

        [TestMethod]
        public void VerificarExistencia_DeveRetornarTrue_QuandoCPFExiste()
        {     
            var cliente = new Cliente
            {
                Nome = "Lucas",
                Sobrenome = "Almeida",
                CPF = "11111111111",
                Email = "lucas.almeida@email.com",
                CEP = "55555-666",
                Cidade = "Fortaleza",
                Estado = "CE",
                Logradouro = "Rua Verificação, 333",
                Nacionalidade = "Brasileiro",
                Telefone = "(85) 95555-4444"
            };
            long id = _boCliente.Incluir(cliente);
           
            bool existe = _boCliente.VerificarExistencia("11111111111");
          
            Assert.IsTrue(existe, "Deve retornar true quando CPF existe");
            
            _boCliente.Excluir(id);
        }

        [TestMethod]
        public void VerificarExistencia_DeveRetornarFalse_QuandoCPFNaoExiste()
        {
            
            string cpfInexistente = "00000000000";
           
            bool existe = _boCliente.VerificarExistencia(cpfInexistente);
           
            Assert.IsFalse(existe, "Deve retornar false quando CPF não existe");
        }

        [TestMethod]
        public void Consultar_DeveRetornarClienteComBeneficiarios()
        {         
            var cliente = new Cliente
            {
                Nome = "Ricardo",
                Sobrenome = "Souza",
                CPF = "22222222222",
                Email = "ricardo.souza@email.com",
                CEP = "66666-777",
                Cidade = "Recife",
                Estado = "PE",
                Logradouro = "Rua Benefício, 444",
                Nacionalidade = "Brasileiro",
                Telefone = "(81) 94444-3333"
            };
            long idCliente = _boCliente.Incluir(cliente);

            var boBeneficiario = new BoBeneficiario();
            var beneficiario = new Beneficiario
            {
                IdCliente = idCliente,
                CPF = "33333333333",
                Nome = "Julia Souza"
            };
            long idBeneficiario = boBeneficiario.Incluir(beneficiario);
           
            var clienteConsultado = _boCliente.Consultar(idCliente);
  
            Assert.IsNotNull(clienteConsultado);
            Assert.IsNotNull(clienteConsultado.Beneficiarios);
            Assert.IsTrue(clienteConsultado.Beneficiarios.Count > 0);
            Assert.IsTrue(clienteConsultado.Beneficiarios.Any(b => b.Id == idBeneficiario));
            
            boBeneficiario.Excluir(idBeneficiario);
            _boCliente.Excluir(idCliente);
        }
    }
}
