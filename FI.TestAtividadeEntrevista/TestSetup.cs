using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace FI.TestAtividadeEntrevista
{
    /// <summary>
    /// Classe de inicialização dos testes - Executa antes de todos os testes
    /// </summary>
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Configura o DataDirectory para apontar para a pasta App_Data do projeto Web
            var appDataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\FI.WebAtividadeEntrevista\App_Data"));
            AppDomain.CurrentDomain.SetData("DataDirectory", appDataPath);

            Console.WriteLine($"DataDirectory configurado para: {appDataPath}");
        }
    }
}
