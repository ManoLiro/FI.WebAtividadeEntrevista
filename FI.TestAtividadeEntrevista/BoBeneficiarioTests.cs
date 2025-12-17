using Microsoft.VisualStudio.TestTools.UnitTesting;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System.Linq;

namespace FI.TestAtividadeEntrevista
{
    [TestClass]
    public class BoBeneficiarioTests
    {
        private BoBeneficiario _boBeneficiario;
        private BoCliente _boCliente;
        private long _idClienteTeste;

        [TestInitialize]
        public void Setup()
        {
            _boBeneficiario = new BoBeneficiario();
            _boCliente = new BoCliente();

            //Criar um cliente de teste para usar nos testes de beneficiário
            var clienteTeste = new Cliente
            {
                Nome = "Cliente",
                Sobrenome = "Teste",
                CPF = "99999999999",
                Email = "cliente.teste@email.com",
                CEP = "88888-999",
                Cidade = "São Paulo",
                Estado = "SP",
                Logradouro = "Rua Teste, 999",
                Nacionalidade = "Brasileiro",
                Telefone = "(11) 99999-9999"
            };
            _idClienteTeste = _boCliente.Incluir(clienteTeste);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Remover o cliente de teste após cada teste
            if (_idClienteTeste > 0)
            {
                _boCliente.Excluir(_idClienteTeste);
            }
        }

        [TestMethod]
        public void Incluir_DeveRetornarIdValido_QuandoBeneficiarioValido()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "12345678901",
                Nome = "João Beneficiário"
            };

            long id = _boBeneficiario.Incluir(beneficiario);

            Assert.IsTrue(id > 0, "O ID retornado deve ser maior que zero");

            _boBeneficiario.Excluir(id);
        }

        [TestMethod]
        public void Incluir_DeveIncluirBeneficiarioComSucesso()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "98765432109",
                Nome = "Maria Beneficiária"
            };

            long id = _boBeneficiario.Incluir(beneficiario);
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);

            Assert.IsTrue(id > 0);
            Assert.IsNotNull(beneficiarios);
            Assert.IsTrue(beneficiarios.Any(b => b.Id == id));
            Assert.IsTrue(beneficiarios.Any(b => b.Nome == "Maria Beneficiária"));

            
            _boBeneficiario.Excluir(id);
        }

        [TestMethod]
        public void Alterar_DeveAtualizarBeneficiario_QuandoDadosValidos()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "11122233344",
                Nome = "Pedro Original"
            };
            long id = _boBeneficiario.Incluir(beneficiario);

            beneficiario.Id = id;
            beneficiario.Nome = "Pedro Atualizado";
            beneficiario.CPF = "11122233345";
            _boBeneficiario.Alterar(beneficiario);

            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);
            var beneficiarioAlterado = beneficiarios.FirstOrDefault(b => b.Id == id);

            Assert.IsNotNull(beneficiarioAlterado);
            Assert.AreEqual("Pedro Atualizado", beneficiarioAlterado.Nome);
            Assert.AreEqual("11122233345", beneficiarioAlterado.CPF);

            _boBeneficiario.Excluir(id);
        }

        [TestMethod]
        public void Excluir_DeveRemoverBeneficiario_QuandoIdExiste()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "55566677788",
                Nome = "Ana Excluir"
            };
            long id = _boBeneficiario.Incluir(beneficiario);

            _boBeneficiario.Excluir(id);
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);

            Assert.IsFalse(beneficiarios.Any(b => b.Id == id), "Beneficiário não deve existir após exclusão");
        }

        [TestMethod]
        public void ListarPorCliente_DeveRetornarListaVazia_QuandoClienteSemBeneficiarios()
        {   
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);
  
            Assert.IsNotNull(beneficiarios);
            Assert.AreEqual(0, beneficiarios.Count, "Lista deve estar vazia quando não há beneficiários");
        }

        [TestMethod]
        public void ListarPorCliente_DeveRetornarTodosBeneficiarios_QuandoClientePossuiBeneficiarios()
        {
            var beneficiario1 = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "33344455566",
                Nome = "Carlos Beneficiário"
            };
            var beneficiario2 = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "77788899900",
                Nome = "Beatriz Beneficiária"
            };
            var beneficiario3 = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "11111111111",
                Nome = "Lucas Beneficiário"
            };

            long id1 = _boBeneficiario.Incluir(beneficiario1);
            long id2 = _boBeneficiario.Incluir(beneficiario2);
            long id3 = _boBeneficiario.Incluir(beneficiario3);
            
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);
            
            Assert.IsNotNull(beneficiarios);
            Assert.AreEqual(3, beneficiarios.Count, "Deve retornar exatamente 3 beneficiários");
            Assert.IsTrue(beneficiarios.Any(b => b.Id == id1));
            Assert.IsTrue(beneficiarios.Any(b => b.Id == id2));
            Assert.IsTrue(beneficiarios.Any(b => b.Id == id3));
            Assert.IsTrue(beneficiarios.Any(b => b.Nome == "Carlos Beneficiário"));
            Assert.IsTrue(beneficiarios.Any(b => b.Nome == "Beatriz Beneficiária"));
            Assert.IsTrue(beneficiarios.Any(b => b.Nome == "Lucas Beneficiário"));
          
            _boBeneficiario.Excluir(id1);
            _boBeneficiario.Excluir(id2);
            _boBeneficiario.Excluir(id3);
        }

        [TestMethod]
        public void ListarPorCliente_DeveRetornarApenasBeneficiariosDoCliente()
        {
            
            //Criando outro cliente além do principal
            var outroCliente = new Cliente
            {
                Nome = "Outro",
                Sobrenome = "Cliente",
                CPF = "88888888888",
                Email = "outro.cliente@email.com",
                CEP = "77777-888",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Logradouro = "Av. Outro, 888",
                Nacionalidade = "Brasileiro",
                Telefone = "(21) 98888-7777"
            };
            long idOutroCliente = _boCliente.Incluir(outroCliente);

            //Adicionando beneficiários para ambos os clientes
            var beneficiarioCliente1 = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "22222222222",
                Nome = "Beneficiário Cliente 1"
            };
            var beneficiarioCliente2 = new Beneficiario
            {
                IdCliente = idOutroCliente,
                CPF = "33333333333",
                Nome = "Beneficiário Cliente 2"
            };

            long idBen1 = _boBeneficiario.Incluir(beneficiarioCliente1);
            long idBen2 = _boBeneficiario.Incluir(beneficiarioCliente2);

            var beneficiariosCliente1 = _boBeneficiario.ListarPorCliente(_idClienteTeste);
            var beneficiariosCliente2 = _boBeneficiario.ListarPorCliente(idOutroCliente);

            Assert.IsNotNull(beneficiariosCliente1);
            Assert.IsNotNull(beneficiariosCliente2);
            Assert.AreEqual(1, beneficiariosCliente1.Count);
            Assert.AreEqual(1, beneficiariosCliente2.Count);
            Assert.IsTrue(beneficiariosCliente1.Any(b => b.IdCliente == _idClienteTeste));
            Assert.IsTrue(beneficiariosCliente2.Any(b => b.IdCliente == idOutroCliente));
            Assert.IsFalse(beneficiariosCliente1.Any(b => b.Id == idBen2));
            Assert.IsFalse(beneficiariosCliente2.Any(b => b.Id == idBen1));

            _boBeneficiario.Excluir(idBen1);
            _boBeneficiario.Excluir(idBen2);
            _boCliente.Excluir(idOutroCliente);
        }

        [TestMethod]
        public void Incluir_DeveVincularBeneficiarioAoClienteCorreto()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "44455566677",
                Nome = "Fernanda Vínculo"
            };

            long id = _boBeneficiario.Incluir(beneficiario);
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);
            var beneficiarioInserido = beneficiarios.FirstOrDefault(b => b.Id == id);

            Assert.IsNotNull(beneficiarioInserido);
            Assert.AreEqual(_idClienteTeste, beneficiarioInserido.IdCliente);
            Assert.AreEqual("Fernanda Vínculo", beneficiarioInserido.Nome);
            Assert.AreEqual("44455566677", beneficiarioInserido.CPF);
            
            _boBeneficiario.Excluir(id);
        }

        [TestMethod]
        public void Beneficiario_DeveManterDadosCompletos()
        {
            var beneficiario = new Beneficiario
            {
                IdCliente = _idClienteTeste,
                CPF = "66677788899",
                Nome = "Roberto Completo"
            };

            long id = _boBeneficiario.Incluir(beneficiario);
            var beneficiarios = _boBeneficiario.ListarPorCliente(_idClienteTeste);
            var beneficiarioRecuperado = beneficiarios.FirstOrDefault(b => b.Id == id);
            
            Assert.IsNotNull(beneficiarioRecuperado);
            Assert.AreEqual(id, beneficiarioRecuperado.Id);
            Assert.AreEqual(_idClienteTeste, beneficiarioRecuperado.IdCliente);
            Assert.AreEqual("66677788899", beneficiarioRecuperado.CPF);
            Assert.AreEqual("Roberto Completo", beneficiarioRecuperado.Nome);

            _boBeneficiario.Excluir(id);
        }
    }
}
