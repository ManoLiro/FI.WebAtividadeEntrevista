# Testes Unitários - FI.AtividadeEntrevista

Este documento descreve os testes unitários criados para o projeto FI.AtividadeEntrevista.

## Estrutura dos Testes

### 1. **BoClienteTests.cs**
Testes unitários para a camada de negócio de Cliente (BoCliente).

#### Testes Implementados:
- `Incluir_DeveRetornarIdValido_QuandoClienteValido` - Testa a inclusão de um novo cliente
- `Consultar_DeveRetornarCliente_QuandoIdExiste` - Testa a consulta de cliente por ID
- `Consultar_DeveRetornarNull_QuandoIdNaoExiste` - Testa consulta com ID inexistente
- `Alterar_DeveAtualizarCliente_QuandoDadosValidos` - Testa a alteração de dados do cliente
- `Excluir_DeveRemoverCliente_QuandoIdExiste` - Testa a exclusão de cliente
- `Listar_DeveRetornarListaDeClientes` - Testa a listagem de todos os clientes
- `Pesquisa_DeveRetornarClientesPaginados` - Testa a pesquisa paginada de clientes
- `VerificarExistencia_DeveRetornarTrue_QuandoCPFExiste` - Testa verificação de CPF existente
- `VerificarExistencia_DeveRetornarFalse_QuandoCPFNaoExiste` - Testa verificação de CPF inexistente
- `Consultar_DeveRetornarClienteComBeneficiarios` - Testa consulta de cliente com seus beneficiários

### 2. **BoBeneficiarioTests.cs**
Testes unitários para a camada de negócio de Beneficiário (BoBeneficiario).

#### Testes Implementados:
- `Incluir_DeveRetornarIdValido_QuandoBeneficiarioValido` - Testa inclusão de beneficiário
- `Incluir_DeveIncluirBeneficiarioComSucesso` - Verifica se beneficiário foi incluído corretamente
- `Alterar_DeveAtualizarBeneficiario_QuandoDadosValidos` - Testa alteração de beneficiário
- `Excluir_DeveRemoverBeneficiario_QuandoIdExiste` - Testa exclusão de beneficiário
- `ListarPorCliente_DeveRetornarListaVazia_QuandoClienteSemBeneficiarios` - Testa listagem vazia
- `ListarPorCliente_DeveRetornarTodosBeneficiarios_QuandoClientePossuiBeneficiarios` - Testa listagem múltipla
- `ListarPorCliente_DeveRetornarApenasBeneficiariosDoCliente` - Verifica isolamento entre clientes
- `Incluir_DeveVincularBeneficiarioAoClienteCorreto` - Testa vínculo correto com cliente
- `Beneficiario_DeveManterDadosCompletos` - Verifica integridade dos dados

**Características dos testes:**
- Cada teste cria um cliente de teste no `[TestInitialize]`
- Cleanup automático no `[TestCleanup]`
- Testes isolados e independentes

### 3. **IntegrationTests.cs** (antigo UnitTest1.cs)
Teste de integração que valida o fluxo completo.

#### Teste Implementado:
- `FluxoCompleto_ClienteComBeneficiarios_DeveExecutarComSucesso` - Testa inclusão de cliente com múltiplos beneficiários e consulta

## Configuração do Banco de Dados

### App.config
O projeto de testes possui um arquivo `App.config` que configura a conexão com o banco de dados:

```xml
<connectionStrings>
  <add name="BancoDeDados" 
       connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BancoDeDados.mdf;Integrated Security=True" />
</connectionStrings>
```

### TestSetup.cs
Classe de inicialização que configura o `DataDirectory` para apontar para o banco de dados do projeto Web:

```csharp
[AssemblyInitialize]
public static void AssemblyInit(TestContext context)
{
    var appDataPath = Path.GetFullPath(Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, 
        @"..\..\..\FI.WebAtividadeEntrevista\App_Data"));
    AppDomain.CurrentDomain.SetData("DataDirectory", appDataPath);
}
```

## Como Executar os Testes

### Via Visual Studio:
1. Abra o **Test Explorer** (Test > Test Explorer)
2. Clique em "Run All" para executar todos os testes
3. Ou clique com botão direito em um teste específico e selecione "Run"

### Via Linha de Comando:
```bash
dotnet test FI.TestAtividadeEntrevista\FI.TestAtividadeEntrevista.csproj
```

### Via Visual Studio Test Console:
```bash
vstest.console.exe FI.TestAtividadeEntrevista\bin\Debug\FI.TestAtividadeEntrevista.dll
```

## Pré-requisitos

1. **SQL Server LocalDB** instalado
2. **Banco de dados** em `FI.WebAtividadeEntrevista\App_Data\BancoDeDados.mdf`
3. **.NET Framework 4.8**
4. **MSTest Framework** (já incluído via NuGet)

## Boas Práticas Implementadas

? **Arrange-Act-Assert (AAA)** - Todos os testes seguem este padrão
? **Cleanup** - Dados de teste são removidos após execução
? **Nomenclatura descritiva** - Nomes de testes explicam o comportamento esperado
? **Isolamento** - Cada teste é independente
? **Testes positivos e negativos** - Cobrem casos de sucesso e falha
? **Setup e Teardown** - Uso de [TestInitialize] e [TestCleanup]

## Cobertura de Testes

### BoCliente:
- ? Incluir
- ? Consultar
- ? Alterar
- ? Excluir
- ? Listar
- ? Pesquisa
- ? VerificarExistencia

### BoBeneficiario:
- ? Incluir
- ? Alterar
- ? Excluir
- ? ListarPorCliente

## Observações Importantes

?? **Atenção:** Os testes interagem com o banco de dados real. Certifique-se de ter um backup antes de executar os testes em produção.

?? **Dica:** Para testes mais isolados, considere usar um banco de dados de testes separado ou implementar mocks para as camadas DAL.

## Troubleshooting

### Erro: "The ConnectionString property has not been initialized"
**Solução:** Verifique se o arquivo `App.config` está presente no projeto de testes e se o `DataDirectory` está configurado corretamente no `TestSetup.cs`.

### Erro: "Cannot attach the file ... as database"
**Solução:** Verifique se o arquivo `BancoDeDados.mdf` existe em `FI.WebAtividadeEntrevista\App_Data\` e se o SQL Server LocalDB está instalado.

### Erro: "CPF já existe"
**Solução:** Execute o cleanup manualmente ou use CPFs únicos em cada execução de teste.

## Melhorias Futuras

- [ ] Implementar testes com Moq para isolar a camada DAL
- [ ] Adicionar testes de performance
- [ ] Implementar testes de validação de dados
- [ ] Adicionar testes de concorrência
- [ ] Criar banco de dados de testes separado
- [ ] Implementar testes de carga
